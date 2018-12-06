﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UnlimitedScrapeWorks.src.Libs;
using UnlimitedScrapeWorks.src.Libs.Aws;
using UnlimitedScrapeWorks.src.Libs.MangaDex;
using UnlimitedScrapeWorks.src.Providers;
using UnlimitedScrapeWorks.src.Sites;

namespace UnlimitedScrapeWorks
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            DotNetEnv.Env.Load();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddTransient<IMangaDexProvider, MangaDexProvider>();
            services.AddScoped<IMangaDexSite, MangaDexSite>();
            services.AddTransient<IGenericParser, GenericParser>();
            services.AddTransient<IMangaParser, MangaParser>();
            services.AddTransient<IChapterParser, ChapterParser>();

            services.AddScoped<IFileHelper, FileHelper>();
            services.AddScoped<IStorageHelper, StorageHelper>();
            services.AddScoped<IS3Wrapper, S3Wrapper>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
