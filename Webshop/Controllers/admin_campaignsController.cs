using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Annytab.Webshop.Controllers
{
    /// <summary>
    /// This class controls the administration of campaigns
    /// </summary>
    [ValidateInput(false)] 
    public class admin_campaignsController : Controller
    {
        #region View methods

        // Get the list of campaigns
        // GET: /admin_campaigns/
        [HttpGet]
        public ActionResult index()
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Get query paramaters
            ViewBag.QueryParams = new QueryParams(Request);

            // Check if the administrator is authorized
            if (Administrator.IsAuthorized(Administrator.GetAllAdminRoles()) == true)
            {
                ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
                ViewBag.AdminSession = true;
                return View();
            }
            else
            {
                // Redirect the user to the start page
                return RedirectToAction("index", "admin_login");
            }

        } // End of the index method

        // Search in the list
        // POST: /admin_campaigns/search
        [HttpPost]
        public ActionResult search(FormCollection collection)
        {
            // Get the keywords
            string keywordString = collection["txtSearch"];
            string sort_field = collection["selectSortField"];
            string sort_order = collection["selectSortOrder"];
            string page_size = collection["selectPageSize"];

            // Return the url with search parameters
            return Redirect("/admin_campaigns?kw=" + Server.UrlEncode(keywordString) + "&sf=" + sort_field + "&so=" + sort_order + "&pz=" + page_size);

        } // End of the search method

        // Get the edit form
        // GET: /admin_campaigns/edit/1?returnUrl=?kw=df&so=ASC
        [HttpGet]
        public ActionResult edit(Int32 id = 0, string returnUrl = "")
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Get query parameters
            ViewBag.QueryParams = new QueryParams(returnUrl);

            // Check if the administrator is authorized
            if (Administrator.IsAuthorized(new string[] { "Administrator", "Editor" }) == true)
            {
                ViewBag.AdminSession = true;
            }
            else if (Administrator.IsAuthorized(Administrator.GetAllAdminRoles()) == true)
            {
                ViewBag.AdminSession = true;
                ViewBag.AdminErrorCode = 1;
                ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
                return View("index");
            }
            else
            {
                // Redirect the user to the start page
                return RedirectToAction("index", "admin_login");
            }

            // Get the default admin language
            Int32 adminLanguageId = currentDomain.back_end_language;

            // Add data to the view
            ViewBag.TranslatedTexts = StaticText.GetAll(adminLanguageId, "id", "ASC");
            ViewBag.Languages = Language.GetAll(currentDomain.back_end_language, "name", "ASC");
            ViewBag.Campaign = Campaign.GetOneById(id);
            ViewBag.ReturnUrl = returnUrl;

            // Create a new empty campaign post if the campaign does not exist
            if (ViewBag.Campaign == null)
            {
                // Add data to the view
                ViewBag.Campaign = new Campaign();
            }

            // Return the edit view
            return View("edit");

        } // End of the edit method

        // Reset the click count for all campaigns or one campaign, set the id to 0 if you want to reset statistics for all campaigns
        // GET: /admin_campaigns/reset_statistics/1?returnUrl=?kw=df&so=ASC
        [HttpGet]
        public ActionResult reset_statistics(Int32 id = 0, string returnUrl = "")
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Get query parameters
            ViewBag.QueryParams = new QueryParams(returnUrl);

            // Check if the administrator is authorized
            if (Administrator.IsAuthorized(new string[] { "Administrator", "Editor" }) == true)
            {
                ViewBag.AdminSession = true;
            }
            else if (Administrator.IsAuthorized(Administrator.GetAllAdminRoles()) == true)
            {
                ViewBag.AdminSession = true;
                ViewBag.AdminErrorCode = 1;
                ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
                return View("index");
            }
            else
            {
                // Redirect the user to the start page
                return RedirectToAction("index", "admin_login");
            }

            // Reset statistics for all campaigns or for just one campaign
            if(id == 0)
            {
                Campaign.ResetStatistics();
            }
            else
            {
                Campaign.UpdateClickCount(id, 0);
            }

            // Return the index view
            return Redirect("/admin_campaigns" + returnUrl);

        } // End of the reset_statistics method

        #endregion

        #region Post methods

        // Update the campaign
        // POST: /admin_campaigns/edit
        [HttpPost]
        public ActionResult edit(FormCollection collection)
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Get the return url
            string returnUrl = collection["returnUrl"];
            ViewBag.QueryParams = new QueryParams(returnUrl);

            // Check if the administrator is authorized
            if (Administrator.IsAuthorized(new string[] { "Administrator", "Editor" }) == true)
            {
                ViewBag.AdminSession = true;
            }
            else if (Administrator.IsAuthorized(Administrator.GetAllAdminRoles()) == true)
            {
                ViewBag.AdminSession = true;
                ViewBag.AdminErrorCode = 1;
                ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
                return View("index");
            }
            else
            {
                // Redirect the user to the start page
                return RedirectToAction("index", "admin_login");
            }

            // Get all the form values
            Int32 id = Convert.ToInt32(collection["txtId"]);
            Int32 languageId = Convert.ToInt32(collection["selectLanguage"]);
            string name = collection["txtName"];
            string url = collection["txtUrl"];
            string categoryName = collection["selectCategory"];
            bool inactive = Convert.ToBoolean(collection["cbInactive"]);
            HttpPostedFileBase campaignImage = Request.Files["campaignImage"];
            
            // Get the default admin language id
            Int32 adminLanguageId = currentDomain.back_end_language;

            // Get translated texts
            KeyStringList tt = StaticText.GetAll(adminLanguageId, "id", "ASC");

            // Get the campaign
            Campaign campaign = Campaign.GetOneById(id);
            bool postExists = true;

            // Check if the campaign exists
            if (campaign == null)
            {
                // Create an empty campaign
                campaign = new Campaign();
                postExists = false;
            }

            // Update values
            campaign.name = name;
            campaign.language_id = languageId;
            campaign.category_name = categoryName;
            campaign.link_url = url;
            campaign.inactive = inactive;

            // Create a error message
            string errorMessage = string.Empty;

            // Check for errors in the campaign
            if(campaign.language_id == 0)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_select_value"), tt.Get("language").ToLower()) + "<br/>";
            }
            if (campaign.name.Length > 50)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("name"), "50") + "<br/>";
            }
            if (campaign.link_url.Length > 200)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("url"), "200") + "<br/>";
            }

            // Check if there is errors
            if (errorMessage == string.Empty)
            {
                // Update the image
                if(campaignImage.ContentLength > 0)
                {
                    UpdateCampaignImage(campaign, campaignImage);
                }

                // Check if we should add or update the campaign
                if (postExists == false)
                {
                    // Add the campaign
                    Campaign.Add(campaign);
                }
                else
                {
                    // Update the campaign
                    Campaign.Update(campaign);
                }

                // Redirect the user to the list
                return Redirect("/admin_campaigns" + returnUrl);
            }
            else
            {
                // Set form values
                ViewBag.ErrorMessage = errorMessage;
                ViewBag.TranslatedTexts = tt;
                ViewBag.Languages = Language.GetAll(currentDomain.back_end_language, "id", "ASC");
                ViewBag.Campaign = campaign;
                ViewBag.ReturnUrl = returnUrl;

                // Return the edit view
                return View("edit");
            }

        } // End of the edit method

        // Delete the campaign
        // POST: /admin_campaigns/delete/1?returnUrl=?kw=sok&qp=2
        [HttpGet]
        public ActionResult delete(Int32 id = 0, string returnUrl = "")
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Get query parameters
            ViewBag.QueryParams = new QueryParams(returnUrl);

            // Check if the administrator is authorized
            if (Administrator.IsAuthorized(new string[] { "Administrator" }) == true)
            {
                ViewBag.AdminSession = true;
            }
            else if (Administrator.IsAuthorized(Administrator.GetAllAdminRoles()) == true)
            {
                ViewBag.AdminSession = true;
                ViewBag.AdminErrorCode = 1;
                ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
                return View("index");
            }
            else
            {
                // Redirect the user to the start page
                return RedirectToAction("index", "admin_login");
            }

            // Get the campaign
            Campaign campaign = Campaign.GetOneById(id);

            // Create an error code variable
            Int32 errorCode = 0;

            // Delete the campaign post and all the connected posts (CASCADE)
            errorCode = Campaign.DeleteOnId(id);

            // Check if there is an error
            if (errorCode != 0)
            {
                ViewBag.AdminErrorCode = errorCode;
                ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
                return View("index");
            }

            // Delete the image
            if(campaign != null)
            {
                DeleteCampaignImage(campaign.image_name);
            }

            // Redirect the user to the list
            return Redirect("/admin_campaigns" + returnUrl);

        } // End of the delete method

        #endregion

        #region Helper methods

        /// <summary>
        /// Update the campaign image
        /// </summary>
        /// <param name="campaign">A reference to the campaign</param>
        /// <param name="campaignImage">The uploaded image</param>
        private void UpdateCampaignImage(Campaign campaign, HttpPostedFileBase campaignImage)
        {
            // Create the directory string
            string campaignDirectory = "/Content/campaigns/images/";

            // Check if the directory exists
            if (System.IO.Directory.Exists(Server.MapPath(campaignDirectory)) == false)
            {
                // Create the directory
                System.IO.Directory.CreateDirectory(Server.MapPath(campaignDirectory));
            }

            // Delete the old image
            if(campaign.image_name != "")
            {
                DeleteCampaignImage(campaign.image_name);
            }

            // Save the new image
            campaign.image_name = System.IO.Path.GetFileName(campaignImage.FileName);
            campaignImage.SaveAs(Server.MapPath(campaignDirectory + campaign.image_name));

        } // End of the UpdateCampaignImage method

        /// <summary>
        /// Delete the campaign image
        /// </summary>
        /// <param name="imageName">The image name</param>
        private void DeleteCampaignImage(string imageName)
        {
            // Create the image url
            string imageUrl = Server.MapPath("/Content/campaigns/images/" + imageName);

            // Delete the image if it exists
            if(System.IO.File.Exists(imageUrl))
            {
                System.IO.File.Delete(imageUrl);
            }

        } // End of the DeleteCampaignImage method

        #endregion

    } // End of the class

} // End of the namespace