﻿using BlogEngine.Core;
using BlogEngine.Core.Providers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.Configuration;
using System.Web.UI.WebControls;
using Page = BlogEngine.Core.Page;

namespace BlogEngine.NET.admin.Pages
{
    public partial class DataMigration : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DdlProviders.Items.Clear();

                BlogProviderSection section = (BlogProviderSection)WebConfigurationManager.GetSection("BlogEngine/blogProvider");
                BlogProviderCollection providers = new BlogProviderCollection();
                ProvidersHelper.InstantiateProviders(section.Providers, providers, typeof(BlogProvider));
                List<Blog> blogs = BlogService.Provider.FillBlogs();

                foreach (Blog blog in blogs)
                {
                    DdlBlogs.Items.Add(new ListItem(blog.Name, blog.Id.ToString()));
                }

                foreach (BlogProvider provider in providers)
                {
                    // Don't add the current provider in use, that would be messy. 
                    if (provider.Name != section.DefaultProvider)
                    DdlProviders.Items.Add(provider.Name);
                }
            }
        }

        protected void BtnConvert_OnClick(object sender, EventArgs e)
        {
            // Get Blog to use
            Blog blog = BlogService.SelectBlog(new Guid(DdlBlogs.SelectedValue));

            // Get ProviderName to convert to
            string providerName = DdlProviders.SelectedValue;
            BlogProvider convertTo = BlogService.Providers[providerName];

            // Insert the Blog Record
            convertTo.InsertBlog(blog);

            // Categories
            // Load up Categories
            List<Category> cats = BlogService.Provider.FillCategories(blog);
            foreach (Category cat in cats)
            {
                convertTo.InsertCategory(cat);
            }

            // Posts
            List<Post> posts = BlogService.Provider.FillPosts();
            foreach (Post post in posts)
            {
                convertTo.InsertPost(post);
            }

            // Pages
            List<Page> pages = BlogService.Provider.FillPages();
            foreach (Page page in pages)
            {
                convertTo.InsertPage(page);
            }

            // Update Settings
            StringDictionary settings = BlogService.Provider.LoadSettings(blog);
            convertTo.SaveSettings(settings);

            // Update Ping Services
            StringCollection pingServices = BlogService.Provider.LoadPingServices();
            convertTo.SavePingServices(pingServices);

            // Update display
            BtnConvert.Enabled = false;
            LblStatus.Text = "Data Copy Complete.";
        }
    }
}