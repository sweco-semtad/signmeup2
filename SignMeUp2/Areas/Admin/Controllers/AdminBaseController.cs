﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SignMeUp2.Data;
using SignMeUp2.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Owin;
using log4net;
using SignMeUp2.Services;

namespace SignMeUp2.Areas.Admin.Controllers
{
    public abstract class AdminBaseController : Controller
    {
        protected SignMeUpDataModel db;

        protected readonly ILog log;

        public AdminBaseController()
        {
            log = LogManager.GetLogger(GetType());
            db = SignMeUpService.Instance.Db; //System.Web.HttpContext.Current.Items["_EntityContext"] as SignMeUpDataModel;
        }

        protected ApplicationUser HamtaUser()
        {
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            return userManager.FindById(User.Identity.GetUserId());
        }

        protected Organisation HamtaOrganisation()
        {
            var user = HamtaUser();
            return db.Organisationer.Find(user.OrganisationsId);
        }

        protected IList<Evenemang> HamtaEvenemangForAnv()
        {
            var user = HamtaUser();

            //var isAdmin = false;
            //foreach (var role in user.Roles)
            //{
            //    isAdmin = role.RoleId == 1;
            //}

            var events = from e in db.Evenemang
                         where e.OrganisationsId == user.OrganisationsId
                         select e;

            // if role admin then show all
            return events.ToList();
        }
    }
}