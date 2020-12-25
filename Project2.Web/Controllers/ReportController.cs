﻿using Project2.Core.Interfaces;
using Project2.Core.Interfaces.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Project2.Web.Common;
using System.Web.Mvc;
using Project2.Core.Models.Entities;

namespace Project2.Web.Controllers
{
    public class ReportController : Controller
    {
        // GET: Report
        private readonly IReportService reportService;
        private readonly ITimeStartService timeStartService;
        private readonly IProjectService projectService;
        private IDataContext dataContext;

        public ReportController(IReportService reportService, IDataContext dataContext, ITimeStartService timeStartService, IProjectService projectService)
        {
            this.reportService = reportService;
            this.dataContext = dataContext;
            this.timeStartService = timeStartService;
            this.projectService = projectService;
        }

        public ActionResult Index()
        {
            var userLogin = (UserLogin)Session["userId"];
            var model = projectService.getListProjectByStudentId(userLogin.id).ToList();
            return View(model);
        }

        [HttpGet]
        public PartialViewResult reportPartialView(int id)
        {
            Project model = null;
            if(id != 0)
            {
                model = projectService.getProjectById(id);
                return PartialView("reportPartialView", model);
            }
            return PartialView("reportPartialView", model);            
        }
        

    }
}