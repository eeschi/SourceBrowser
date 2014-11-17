﻿namespace SourceBrowser.Site.Controllers
{
    using System;
    using System.Web.Mvc;

    using SourceBrowser.Site.Repositories;

    public class BrowseController : Controller
    {
        // GET: Browse
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LookupFile(string id)
        {
            if (BrowserRepository.IsFile(id))
            {
                string githubUser;
                string githubRepo;
                string solutionName;
                string fileName;
                BrowserRepository.GetFolderInfo(id, out githubUser, out githubRepo, out solutionName, out fileName);

                if (!string.IsNullOrEmpty(fileName))
                {
                    var docInfo = BrowserRepository.FindFile(id);
                    var viewModel = BrowserRepository.SetUpFileStructure(docInfo, githubUser, githubRepo, solutionName, fileName);
                    return View("LookupFile", viewModel);
                }
            }

            return View("LookupError");
        }

        public ActionResult LookupFolder(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                var users = BrowserRepository.GetAllGithubUsers();
                ViewBag.Users = users;
                return View("Index");
            }

            string githubUser;
            string githubRepo;
            string solutionName;
            string fileName;
            BrowserRepository.GetFolderInfo(id, out githubUser, out githubRepo, out solutionName, out fileName);

            try
            {
                if (!string.IsNullOrEmpty(solutionName))
                {
                    var viewModel = BrowserRepository.SetUpSolutionStructure(githubUser, githubRepo, solutionName);
                    return View("LookupFolder", viewModel);
                }
                if (!string.IsNullOrEmpty(githubRepo))
                {
                    var viewModel = BrowserRepository.GetRepoStructure(githubUser, githubRepo);
                    return this.View("LookupRepo", viewModel);
                }
                if (!string.IsNullOrEmpty(githubUser))
                {
                    var viewModel = BrowserRepository.GetUserStructure(githubUser);
                    return this.View("LookupUser", viewModel);
                }
                return this.View("LookupError");
            }
            catch
            {
                return View("LookupError");
            }
        }
    }
}