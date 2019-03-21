/*
 * Honeywell, UAV Flight Designer.
 * Copyright 2019 Honeywell International Inc.
 *
 * HONEYWELL-CONFIDENTIAL: this copyrighted work and all information are the property of Honeywell,
 * contain trade secrets and may not, in whole or in part, be used, duplicated, or disclosed for
 * any purpose without prior written permission of Honeywell. All Rights Reserved.
 *
 * The methods and technique described herein are considered trade secrets and/or confidential.
 * Reproduction or distribution, in whole or in part, is forbidden except by express written
 * permission of the owners.
 */

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using VerizonDigital.CDN.TokenProvider.Sample;

namespace Honeywell.FlightDesigner.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);

            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddDebug();
                    logging.AddConsole();
                })
                .Build();
        }
    }
}
