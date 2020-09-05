using SecurityPrivacyTest.Models;
using SecurityPrivacyTest.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SecurityPrivacyTest.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [Route("AccountModal")]
        public ActionResult AccountModel()
        {
            return PartialView("~/Views/Modals/_AddAccountModal.cshtml");
        }

        [Route("SaveAccount")]
        public JsonResult SaveAccount(string ip, string os, string country, string binary)
        {
            try
            {
                CRUDService db = new Services.CRUDService();

                AccountModel account = new AccountModel
                {
                    Ip = ip,
                    OS = os,
                    Country = country,
                    Binary = binary
                };

                db.InsertAccount("Accounts", account);

                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
        }

        [Route("GetAccounts")]
        public ActionResult GetAccounts()
        {
            CRUDService db = new Services.CRUDService();

            var accoutns = db.LoadRecords<AccountModel>("Accounts");

            return PartialView("~/Views/Home/_AccountsTable.cshtml", accoutns);
        }

        [Route("EditAccountModal")]
        public ActionResult EditAccountModal(Guid id)
        {
            CRUDService db = new Services.CRUDService();

            var account = db.LoadRecordById<AccountModel>("Accounts", id);

            return PartialView("~/Views/Modals/_EditAccountModal.cshtml", account);
        }

        [Route("DeleteAccount")]
        public JsonResult DeleteAccount(Guid id)
        {
            CRUDService db = new Services.CRUDService();

            db.DeleteRecords<AccountModel>("Accounts", id);

            return Json("success", JsonRequestBehavior.AllowGet);
        }


        [Route("UpsertAccount")]
        public JsonResult UpsertAccount(Guid id, string ip, string os, string country, string binary)
        {
            try
            {
                CRUDService db = new Services.CRUDService();

                var account = db.LoadRecordById<AccountModel>("Accounts", id);

                account.Ip = ip;
                account.OS = os;
                account.Country = country;
                account.Binary = binary;

                db.UpsertRecord("Accounts",id,account);

                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var err = e;
                return Json("error", JsonRequestBehavior.AllowGet);
            }
        }

        [Route("BinaryCheck")]
        public JsonResult BinaryCheck(string binary)
        {
            if(binary.Count(c => (c == '1')) != binary.Count(c => (c == '0')))
            {
                return Json(new { isSuccess = "false" });
            }

            for (int i = 1; i < binary.Length; i++)
            {
                var prefx = binary.Substring(0, i);

                if (prefx.Count(c => (c == '1')) < prefx.Count(c => (c == '0')))
                {
                    return Json(new { isSuccess = "false" });
                }
            }

            return Json(new { isSuccess = "true" });
        }
    }
}