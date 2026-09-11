using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KsDumper11
{
    public class KduProvider
    {
        public int ProviderIndex { get; set; }

        public string ProviderName { get; set; }
        public string DriverName { get; set; }
        public string DeviceName { get; set; }
        public string SignerName { get; set; }
        public bool IsWHQL_Signed { get; set; }
        public string ShellcodeSupportMask { get; set; }

        public string MaxWindowsBuild { get; set; }
        public string MinWindowsBuild { get; set; }

        // --- Fields introduced by KDU v1.5.0's "-list" output ---
        public string Advisory { get; set; }

        public string ImageSize { get; set; }

        public string FileHashSHA1 { get; set; }

        public string AuthenticodeHashSHA1 { get; set; }

        public string PageHashSHA1 { get; set; }

        public string PageHashSHA256 { get; set; }
        // --------------------------------------------------------

        public string[] ExtraInfo { get; set; }

        public bool IsNonWorking
        {
            get
            {
                return this.ProviderName.Contains("NOT WORKING");
            }
        }

        public bool IsWorking
        {
            get
            {
                return this.ProviderName.Contains("WORKING");
            }
        }


        public KduProvider()
        {
        }

        public KduProvider(string provider)
        {
            processProvider(provider);
        }

        private void processProvider(string prov)
        {
            string[] lines = prov.Split('\n');

            // Locate the provider header line, e.g. " 0, ResourceId # 103".
            // We cannot assume it is lines[0]: when the raw KDU output is split on the
            // literal "Provider #" token, trailing metadata (such as the previous
            // provider's "Signer:" lines) can leak into the front of this block, and
            // the header itself may be prefixed by whitespace only. Scanning for the
            // "<digits>, ResourceId" pattern is robust against both cases.
            int headerLineIndex = -1;
            int parsedProviderIndex = -1;

            for (int i = 0; i < lines.Length; i++)
            {
                string candidate = lines[i].Trim();
                if (string.IsNullOrEmpty(candidate))
                {
                    continue;
                }

                Match headerMatch = Regex.Match(candidate, @"^(\d+)\s*,\s*ResourceId");
                if (headerMatch.Success)
                {
                    headerLineIndex = i;
                    parsedProviderIndex = int.Parse(headerMatch.Groups[1].Value);
                    break;
                }
            }

            if (headerLineIndex < 0 || headerLineIndex + 1 >= lines.Length)
            {
                // Malformed block - record it as unparsed rather than throwing.
                ProviderIndex = -1;
                ProviderName = "(Unparsed Provider)";
                return;
            }

            ProviderIndex = parsedProviderIndex;

            // The line immediately after the header carries the provider description:
            //   KDU 1.4.4: "CVE-2015-2291, DriverName \"NalDrv\", DeviceName \"Nal\""
            //   KDU 1.5.0: "Intel NAL driver, DriverName \"NalDrv\", DeviceName \"Nal\""
            //              or  "ASRock Polychrome RGB, multiple CVE ids, DriverName \"GLCKIo2\", DeviceName \"GLCKIo2\""
            // Use regex so any extra commas inside the provider name do not break parsing.
            string provLine = lines[headerLineIndex + 1];

            Match driverMatch = Regex.Match(provLine, "DriverName\\s+\"([^\"]*)\"");
            Match deviceMatch = Regex.Match(provLine, "DeviceName\\s+\"([^\"]*)\"");

            if (driverMatch.Success)
            {
                DriverName = driverMatch.Groups[1].Value;
            }

            if (deviceMatch.Success)
            {
                DeviceName = deviceMatch.Groups[1].Value;
            }

            // ProviderName is everything in front of the "DriverName" tag.
            int driverTagIndex = provLine.IndexOf("DriverName", StringComparison.Ordinal);
            if (driverTagIndex > 0)
            {
                int commaBeforeDriver = provLine.LastIndexOf(',', driverTagIndex);
                if (commaBeforeDriver > 0)
                {
                    ProviderName = provLine.Substring(0, commaBeforeDriver).Trim();
                }
                else
                {
                    ProviderName = provLine.Substring(0, driverTagIndex).Trim().TrimEnd(',');
                }
            }
            else
            {
                ProviderName = provLine.Trim();
            }

            // Reset optional fields so a re-parse starts clean.
            Advisory = null;
            SignerName = null;
            ImageSize = null;
            FileHashSHA1 = null;
            AuthenticodeHashSHA1 = null;
            PageHashSHA1 = null;
            PageHashSHA256 = null;
            ShellcodeSupportMask = null;
            MaxWindowsBuild = null;
            MinWindowsBuild = null;
            IsWHQL_Signed = false;

            List<string> extraInfoLines = new List<string>();
            bool inCapabilities = false;

            for (int i = headerLineIndex + 2; i < lines.Length; i++)
            {
                string line = lines[i].Trim();

                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }

                // Safety: stop if the next provider leaked into this part.
                if (line.StartsWith("Provider #"))
                {
                    break;
                }

                // KDU 1.5.0 adds an "Advisory" line. Not every provider has one.
                if (line.StartsWith("Advisory: "))
                {
                    Advisory = line.Substring("Advisory: ".Length).Trim().Trim('"');
                    continue;
                }

                // KDU 1.5.0 renamed "Signed by:" to "Signer:".
                if (line.StartsWith("Signer: "))
                {
                    SignerName = line.Substring("Signer: ".Length).Trim().Trim('"');
                    continue;
                }

                // Legacy tag (KDU 1.4.4 and earlier).
                if (line.StartsWith("Signed by: "))
                {
                    SignerName = line.Substring("Signed by: ".Length).Trim().Trim('"');
                    continue;
                }

                if (line.StartsWith("Image size: "))
                {
                    ImageSize = line.Substring("Image size: ".Length).Trim();
                    continue;
                }

                if (line.StartsWith("File hash (SHA1): "))
                {
                    FileHashSHA1 = line.Substring("File hash (SHA1): ".Length).Trim();
                    continue;
                }

                if (line.StartsWith("Authenticode hash (SHA1): "))
                {
                    AuthenticodeHashSHA1 = line.Substring("Authenticode hash (SHA1): ".Length).Trim();
                    continue;
                }

                if (line.StartsWith("Page hash (SHA1): "))
                {
                    PageHashSHA1 = line.Substring("Page hash (SHA1): ".Length).Trim();
                    continue;
                }

                if (line.StartsWith("Page hash (SHA256): "))
                {
                    PageHashSHA256 = line.Substring("Page hash (SHA256): ".Length).Trim();
                    continue;
                }

                if (line.StartsWith("Shellcode support mask: "))
                {
                    ShellcodeSupportMask = line.Substring("Shellcode support mask: ".Length).Trim().Trim('"');
                    continue;
                }

                // Capability flag lives inside the "Provider capabilities" block,
                // but is scanned globally for backwards compatibility.
                if (line.Contains("Driver is WHQL signed"))
                {
                    IsWHQL_Signed = true;
                    continue;
                }

                if (line.StartsWith("Minimum supported Windows build: "))
                {
                    MinWindowsBuild = line.Substring("Minimum supported Windows build: ".Length).Trim();
                    continue;
                }

                if (line.StartsWith("Maximum Windows build undefined"))
                {
                    MaxWindowsBuild = "No Restrictions";
                    continue;
                }

                if (line.StartsWith("Maximum supported Windows build: "))
                {
                    MaxWindowsBuild = line.Substring("Maximum supported Windows build: ".Length).Trim();
                    continue;
                }

                // Extra info block starts with "Provider capabilities:" and contains
                // "->" bullets plus an optional "Based on:" footer.
                if (line.StartsWith("Provider capabilities:"))
                {
                    inCapabilities = true;
                    extraInfoLines.Add(line);
                    continue;
                }

                if (inCapabilities && (line.StartsWith("->") || line.StartsWith("Based on: ")))
                {
                    extraInfoLines.Add(line);
                    continue;
                }
            }

            ExtraInfo = extraInfoLines.ToArray();
        }
    }
}
