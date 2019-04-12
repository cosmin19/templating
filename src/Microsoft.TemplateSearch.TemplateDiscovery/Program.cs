using System;
using Microsoft.TemplateSearch.TemplateDiscovery.Nuget;
using Microsoft.TemplateSearch.TemplateDiscovery.PackChecking;
using Microsoft.TemplateSearch.TemplateDiscovery.PackChecking.Reporting;
using Microsoft.TemplateSearch.TemplateDiscovery.Results;

namespace Microsoft.TemplateSearch.TemplateDiscovery
{
    class Program
    {
        private static readonly bool _defaultRunOnlyOnePage = false;
        private static readonly int _defaultPageSize = 100;

        private static readonly string _basePathFlag = "--basePath";
        private static readonly string _pageSizeFlag = "--pageSize";
        private static readonly string _runOnlyOnePageFlag = "--onePage";
        private static readonly string _previousOutputBasePathFlag = "--previousOutput";

        static void Main(string[] args)
        {
            // setup the config with defaults
            ScraperConfig config = new ScraperConfig()
            {
                PageSize = _defaultPageSize,
                RunOnlyOnePage = _defaultRunOnlyOnePage
            };

            if (!TryParseArgs(args, config) || string.IsNullOrEmpty(config.BasePath))
            {
                // base path is the only required arg.
                ShowUsageMessage();
                return;
            }

            PackSourceChecker packSourceChecker;

            // if or when we add other sources to scrape, input args can control which execute(s).
            if (true)
            {
                if (!NugetPackScraper.TryCreateDefaultNugetPackScraper(config, out packSourceChecker))
                {
                    Console.WriteLine("Unable to create the NugetPackScraper.");
                    return;
                }
            }
            else
            {
                throw new NotImplementedException("no checker for the input options");
            }

            PackSourceCheckResult checkResults = packSourceChecker.CheckPackages();
            PackCheckResultReportWriter.TryWriteResults(config.BasePath, checkResults);
        }

        private static void ShowUsageMessage()
        {
            Console.WriteLine("Invalid inputs");
            Console.WriteLine();
            Console.WriteLine("Valid args:");
            Console.WriteLine($"{_basePathFlag} - the root dir for output for this run.");
            Console.WriteLine($"{_previousOutputBasePathFlag} - the root dir for output of a previous run. If specified, uses this output to filter packs known to not contain templates.");
            Console.WriteLine($"{_pageSizeFlag} - (debugging) the chuck size for interactions with the source.");
            Console.WriteLine($"{_runOnlyOnePageFlag} - (debugging) only process one page of template packs.");
        }

        private static bool TryParseArgs(string[] args, ScraperConfig config)
        {
            int index = 0;

            while (index < args.Length)
            {
                if (string.Equals(args[index], _basePathFlag, StringComparison.Ordinal))
                {
                    if (TryGetFlagValue(args, index, out string basePath))
                    {
                        config.BasePath = basePath;
                        index += 2;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (string.Equals(args[index], _pageSizeFlag, StringComparison.Ordinal))
                {
                    if (TryGetFlagValue(args, index, out string pageSizeString) && int.TryParse(pageSizeString, out int pageSize))
                    {
                        config.PageSize = pageSize;
                        index += 2;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (string.Equals(args[index], _previousOutputBasePathFlag, StringComparison.Ordinal))
                {
                    if (TryGetFlagValue(args, index, out string previousOutputBasePath))
                    {
                        config.PreviousRunBasePath = previousOutputBasePath;
                        index += 2;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (string.Equals(args[index], _runOnlyOnePageFlag, StringComparison.Ordinal))
                {
                    config.RunOnlyOnePage = true;
                    ++index;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        private static bool TryGetFlagValue(string[] args, int index, out string value)
        {
            if (index < args.Length && !args[index + 1].StartsWith("-"))
            {
                value = args[index + 1];
                return true;
            }

            value = null;
            return false;
        }
    }
}