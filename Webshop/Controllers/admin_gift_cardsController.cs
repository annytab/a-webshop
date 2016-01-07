using System;
using System.Globalization;
using System.Web.Mvc;

namespace Annytab.Webshop.Controllers
{
    /// <summary>
    /// This class controls the administration of gift cards
    /// </summary>
    [ValidateInput(false)] 
    public class admin_gift_cardsController : Controller
    {
        #region View methods

        // Get a list of gift cards
        // GET: /admin_gift_cards/
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
        // POST: /admin_gift_cards/search
        [HttpPost]
        public ActionResult search(FormCollection collection)
        {
            // Get the keywords
            string keywordString = collection["txtSearch"];
            string sort_field = collection["selectSortField"];
            string sort_order = collection["selectSortOrder"];
            string page_size = collection["selectPageSize"];

            // Return the url with search parameters
            return Redirect("/admin_gift_cards?kw=" + Server.UrlEncode(keywordString) + "&sf=" + sort_field + "&so=" + sort_order + "&pz=" + page_size);

        } // End of the search method

        // Get the edit form
        // GET: /admin_gift_cards/edit/SXXXSWEEE?returnUrl=?kw=df&so=ASC
        [HttpGet]
        public ActionResult edit(string id = "", string returnUrl = "")
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

            // Add data to the view
            ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
            ViewBag.Languages = Language.GetAll(currentDomain.back_end_language, "name", "ASC");
            ViewBag.GiftCard = GiftCard.GetOneById(id);
            ViewBag.ReturnUrl = returnUrl;

            // Create a new empty gift card post if the gift card does not exist
            if (ViewBag.GiftCard == null)
            {
                // Add data to the view
                ViewBag.GiftCard = new GiftCard();
            }

            // Return the edit view
            return View("edit");

        } // End of the edit method

        // Get orders for the gift card
        // GET: /admin_gift_cards/orders/ZZSSSQSCXXX?returnUrl=?kw=df&so=ASC
        [HttpGet]
        public ActionResult orders(string id = "", string returnUrl = "")
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Get query parameters
            ViewBag.QueryParams = new QueryParams(returnUrl);

            // Check if the administrator is authorized
            if (Administrator.IsAuthorized(new string[] { "Administrator", "Editor", "Translator" }) == true)
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

            // Get the gift card
            GiftCard giftCard = GiftCard.GetOneById(id);

            // Check if the gift card is null
            if (giftCard == null)
            {
                return RedirectToAction("index");
            }

            // Set form data
            ViewBag.CurrentDomain = currentDomain;
            ViewBag.GiftCard = giftCard;
            ViewBag.OrderGiftCards = OrderGiftCard.GetByGiftCardId(giftCard.id, "order_id", "ASC");
            ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
            ViewBag.CultureInfo = Tools.GetCultureInfo(Language.GetOneById(currentDomain.back_end_language));
            ViewBag.ReturnUrl = returnUrl;

            // Return the view
            return View();

        } // End of the orders method

        #endregion

        #region Post methods

        // Update the gift card
        // POST: /admin_gift_cards/edit
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
            string id = collection["txtId"];
            Int32 language_id = Convert.ToInt32(collection["selectLanguage"]);
            string currency_code = collection["selectCurrency"];
            decimal amount = 0;
            decimal.TryParse(collection["txtAmount"].Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out amount);
            DateTime end_date = Convert.ToDateTime(collection["txtEndDate"]);

            // Get translated texts
            KeyStringList tt = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");

            // Get the gift card
            GiftCard giftCard = GiftCard.GetOneById(id);
            bool postExists = true;

            // Check if the gift card exists
            if (giftCard == null)
            {
                // Create an empty gift card
                giftCard = new GiftCard();
                postExists = false;
            }

            // Update values
            giftCard.id = id;
            giftCard.language_id = language_id;
            giftCard.currency_code = currency_code;
            giftCard.amount = amount;
            giftCard.end_date = AnnytabDataValidation.TruncateDateTime(end_date);

            // Create a error message
            string errorMessage = string.Empty;

            if (giftCard.id.Length == 0 || giftCard.id.Length > 50)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_certain_length"), tt.Get("id"), "1", "50") + "<br/>";
            }
            if (giftCard.language_id == 0)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_select_value"), tt.Get("language").ToLower()) + "<br/>";
            }
            if (giftCard.currency_code == "")
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_select_value"), tt.Get("currency").ToLower()) + "<br/>";
            }
            if (giftCard.amount < 0 || giftCard.amount > 999999999999M)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_range"), tt.Get("amount"), "999 999 999 999") + "<br/>";
            }

            // Check if there is errors
            if (errorMessage == string.Empty)
            {
                // Check if we should add or update the gift card
                if (postExists == false)
                {
                    // Add the gift card
                    GiftCard.Add(giftCard);
                }
                else
                {
                    // Update the gift card
                    GiftCard.Update(giftCard);
                }

                // Redirect the user to the list
                return Redirect("/admin_gift_cards" + returnUrl);
            }
            else
            {
                // Set form values
                ViewBag.ErrorMessage = errorMessage;
                ViewBag.TranslatedTexts = tt;
                ViewBag.Languages = Language.GetAll(currentDomain.back_end_language, "id", "ASC");
                ViewBag.GiftCard = giftCard;
                ViewBag.ReturnUrl = returnUrl;

                // Return the edit view
                return View("edit");
            }

        } // End of the edit method

        // Delete the gift card
        // POST: /admin_gift_cards/delete/XXXSSSWWSS?returnUrl=?kw=sok&qp=2
        [HttpGet]
        public ActionResult delete(string id = "", string returnUrl = "")
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

            // Create an error code variable
            Int32 errorCode = 0;

            // Delete the gift card post and all the connected posts (CASCADE)
            errorCode = GiftCard.DeleteOnId(id);

            // Check if there is an error
            if (errorCode != 0)
            {
                ViewBag.AdminErrorCode = errorCode;
                ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
                return View("index");
            }

            // Redirect the user to the list
            return Redirect("/admin_gift_cards" + returnUrl);

        } // End of the delete method

        #endregion

    } // End of the class

} // End of the namespace