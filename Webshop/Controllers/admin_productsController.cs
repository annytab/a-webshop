using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Annytab.Webshop.Controllers
{
    /// <summary>
    /// This class handles the product administration
    /// </summary>
    [ValidateInput(false)]
    public class admin_productsController : Controller
    {
        #region View methods

        // Get the list of products
        // GET: /admin_products/
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
                ViewBag.AdminSession = true;
                ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
                return View();
            }
            else
            {
                // Redirect the user to the start page
                return RedirectToAction("index", "admin_login");
            }

        } // End of the index method

        // Search in the list
        // POST: /admin_products/search
        [HttpPost]
        public ActionResult search(FormCollection collection)
        {
            // Get the form data
            string viewString = collection["view"];
            string returnUrl = collection["returnUrl"];
            string keywordString = collection["txtSearch"];
            string sort_field = collection["selectSortField"];
            string sort_order = collection["selectSortOrder"];
            string page_size = collection["selectPageSize"];
            string redirectUrl = "~/admin_products?kw=" + Server.UrlEncode(keywordString) + "&sf=" + sort_field + "&so=" + sort_order + "&pz=" + page_size;

            // Check if we should search among accessories
            if(viewString == "accessories")
            {
                string mainProductId = collection["hiddenMainProductId"];
                redirectUrl = "~/admin_products/accessories/" + mainProductId + "?returnUrl=" + Server.UrlEncode(returnUrl) + "&kw=" + Server.UrlEncode(keywordString);
            }
            else if (viewString == "bundles")
            {
                string mainProductId = collection["hiddenMainProductId"];
                redirectUrl = "~/admin_products/bundle_structure/" + mainProductId + "?returnUrl=" +  Server.UrlEncode(returnUrl) + "&kw=" + Server.UrlEncode(keywordString);
            }

            // Return the url with search parameters
            return Redirect(redirectUrl);

        } // End of the search method

        // Get the edit form
        // GET: /admin_products/edit
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

            // Get the administrator default language
            Int32 adminLanguageId = currentDomain.back_end_language;

            // Get the product
            ViewBag.Product = Product.GetOneById(id, adminLanguageId);

            // Get translated texts
            ViewBag.TranslatedTexts = StaticText.GetAll(adminLanguageId, "id", "ASC");

            // Get units
            ViewBag.Units = Unit.GetAll(adminLanguageId, "name", "ASC");

            // Get all the product option types
            List<ProductOptionType> productOptionTypes = ProductOptionType.GetByProductId(id, adminLanguageId);
            productOptionTypes.AddRange(ProductOptionType.GetOtherOptionTypes(productOptionTypes, adminLanguageId));

            // Add product option types and product options
            if (ViewBag.Product != null)
            {
                // Create a dictionary
                Dictionary<Int32, List<ProductOption>> productOptions = new Dictionary<Int32, List<ProductOption>>(productOptionTypes.Count);

                // Loop all the product option types
                for (int i = 0; i < productOptionTypes.Count; i++)
                {
                    // Get the product options for the option type
                    List<ProductOption> listProductOptions = null;
                    if(productOptionTypes[i].id != 0)
                    {
                        listProductOptions = ProductOption.GetByProductOptionTypeId(productOptionTypes[i].id, adminLanguageId);
                        listProductOptions.AddRange(ProductOption.GetOtherOptions(listProductOptions, productOptionTypes[i].option_type_id, adminLanguageId));
                    }
                    else
                    {
                        listProductOptions = ProductOption.GetOtherOptions(null, productOptionTypes[i].option_type_id, adminLanguageId);
                    }

                    // Add to the dictionary
                    productOptions.Add(productOptionTypes[i].option_type_id, listProductOptions);
                }

                // Add data to the view
                ViewBag.ProductOptionTypes = productOptionTypes;
                ViewBag.ProductOptions = productOptions;
            }
            else
            {
                // Get option types other than product option types
                productOptionTypes = ProductOptionType.GetOtherOptionTypes(null, adminLanguageId);

                // Create a dictionary
                Dictionary<Int32, List<ProductOption>> productOptions = new Dictionary<Int32, List<ProductOption>>(productOptionTypes.Count);

                for (int i = 0; i < productOptionTypes.Count; i++)
                {
                    // Get the product options for the option type
                    List<ProductOption> listProductOptions = ProductOption.GetOtherOptions(null, productOptionTypes[i].option_type_id, adminLanguageId);

                    // Add to the dictionary
                    productOptions.Add(productOptionTypes[i].option_type_id, listProductOptions);
                }

                // Add data to the view
                ViewBag.Product = new Product();
                ViewBag.ProductOptionTypes = productOptionTypes;
                ViewBag.ProductOptions = productOptions;
            }

            // Get the return url
            ViewBag.ReturnUrl = returnUrl;

            // Return the edit view
            return View("edit");

        } // End of the edit method

        // Get the images form
        // GET: /admin_products/images
        [HttpGet]
        public ActionResult images(Int32 id = 0, string returnUrl = "")
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

            // Get the default admin language id
            Int32 adminLanguageId = currentDomain.back_end_language;

            // Get the language id
            int languageId = 0;
            if (Request.Params["lang"] != null)
            {
                Int32.TryParse(Request.Params["lang"], out languageId);
            }

            // Add data to the form
            ViewBag.LanguageId = languageId;
            ViewBag.Languages = Language.GetAll(adminLanguageId, "name", "ASC");
            ViewBag.Product = Product.GetOneById(id, adminLanguageId);
            ViewBag.MainImageUrl = GetMainImageUrl(id, languageId);
            ViewBag.OtherImages = Tools.GetOtherProductImageUrls(id, languageId, true);
            ViewBag.TranslatedTexts = StaticText.GetAll(adminLanguageId, "id", "ASC");
            ViewBag.ReturnUrl = returnUrl;

            // Return the view
            if (ViewBag.Product != null)
            {
                return View("images");
            }
            else
            {
                return Redirect("/admin_products" + returnUrl);
            }

        } // End of the images method

        // Get the accessories form
        // GET: /admin_products/accessories
        [HttpGet]
        public ActionResult accessories(Int32 id = 0, string returnUrl = "")
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

            // Get the default admin language id
            Int32 adminLanguageId = currentDomain.back_end_language;

            // Add data to the form
            ViewBag.MainProductId = id;
            ViewBag.Accessories = ProductAccessory.GetByProductId(id, adminLanguageId, "title", "ASC");
            ViewBag.TranslatedTexts = StaticText.GetAll(adminLanguageId, "id", "ASC");
            ViewBag.ReturnUrl = returnUrl;

            // Return the view
            return View();

        } // End of the accessories method

        // Get the bundle structure form
        // GET: /admin_products/bundle_structure
        [HttpGet]
        public ActionResult bundle_structure(Int32 id = 0, string returnUrl = "")
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

            // Get the default admin language id
            Int32 adminLanguageId = currentDomain.back_end_language;

            // Add data to the form
            ViewBag.BundleProductId = id;
            ViewBag.BundleItems = ProductBundle.GetByBundleProductId(id);
            ViewBag.TranslatedTexts = StaticText.GetAll(adminLanguageId, "id", "ASC");
            ViewBag.ReturnUrl = returnUrl;

            // Return the view
            return View();

        } // End of the bundle_structure method

        // Get the translate form
        // GET: /admin_products/translate
        [HttpGet]
        public ActionResult translate(Int32 id = 0, string returnUrl = "")
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

            // Get the default language id
            Int32 adminLanguageId = currentDomain.back_end_language;

            // Get the language id
            int languageId = 0;
            if (Request.Params["lang"] != null)
            {
                Int32.TryParse(Request.Params["lang"], out languageId);
            }

            // Add data to the form
            ViewBag.LanguageId = languageId;
            ViewBag.Languages = Language.GetAll(adminLanguageId, "name", "ASC");
            ViewBag.StandardProduct = Product.GetOneById(id, adminLanguageId);
            ViewBag.TranslatedProduct = Product.GetOneById(id, languageId);
            ViewBag.TranslatedTexts = StaticText.GetAll(adminLanguageId, "id", "ASC");
            ViewBag.ReturnUrl = returnUrl;

            // Return the view
            if (ViewBag.StandardProduct != null)
            {
                return View("translate");
            }
            else
            {
                return Redirect("/admin_products" + returnUrl);
            }

        } // End of the translate method

        // Get the downloadable files form
        // GET: /admin_products/files
        [HttpGet]
        public ActionResult files(Int32 id = 0, string returnUrl = "")
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

            // Get the default admin language
            Int32 adminLanguageId = currentDomain.back_end_language;

            // Get the language id
            int languageId = 0;
            if (Request.Params["lang"] != null)
            {
                Int32.TryParse(Request.Params["lang"], out languageId);
            }

            // Add data to the form
            ViewBag.LanguageId = languageId;
            ViewBag.Languages = Language.GetAll(adminLanguageId, "name", "ASC");
            ViewBag.Product = Product.GetOneById(id, adminLanguageId);
            ViewBag.DownloadableFiles = Tools.GetDownloadableFiles(id, languageId, true);
            ViewBag.TranslatedTexts = StaticText.GetAll(adminLanguageId, "id", "ASC");
            ViewBag.ReturnUrl = returnUrl;

            // Return the view
            if (ViewBag.Product != null)
            {
                return View("files");
            }
            else
            {
                return Redirect("/admin_products" + returnUrl);
            }

        } // End of the files method

        // Reset statistics for all products or a specific product, set the id to 0 if you want to reset statistics for all products
        // GET: /admin_products/reset_statistics/1?returnUrl=?kw=df&so=ASC
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

            // Reset statistics for all products or just one product
            if (id == 0)
            {
                Product.ResetStatistics();
            }
            else
            {
                Product.ResetStatistics(id);
            }

            // Return the index view
            return Redirect("/admin_products" + returnUrl);

        } // End of the reset_statistics method

        #endregion

        #region Post methods

        // Update the product
        // POST: /admin_product/edit
        [HttpPost]
        public ActionResult edit(FormCollection collection)
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Get query parameters
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

            // Get form values (Product)
            Int32 productId = Convert.ToInt32(collection["txtId"]);
            Int32 categoryId = Convert.ToInt32(collection["selectCategory"]);
            string title = collection["txtTitle"];
            string productCode = collection["txtProductCode"];
            string manufacturer_code = collection["txtManufacturerCode"];
            string gtin = collection["txtGtin"];
            decimal price = 0;
            decimal.TryParse(collection["txtPrice"].Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out price);
            decimal freight = 0;
            decimal.TryParse(collection["txtFreight"].Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out freight);
            Int32 unitId = Convert.ToInt32(collection["selectUnit"]);
            decimal discount = 0;
            decimal.TryParse(collection["txtDiscount"].Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out discount);
            decimal mountTimeHours = 0;
            decimal.TryParse(collection["txtMountTimeHours"].Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out mountTimeHours);
            bool from_price = Convert.ToBoolean(collection["cbFromPrice"]);
            string brand = collection["txtBrand"];
            string supplierErpId = collection["txtSupplierErpId"];
            string description = collection["txtDescription"];
            string extra_content = collection["txtExtraContent"];
            string metaDescription = collection["txtMetaDescription"];
            string metaKeywords = collection["txtMetaKeywords"];
            string pageName = collection["txtPageName"];
            string condition = collection["selectCondition"];
            string variant_image_filename = collection["txtVariantImageFileName"];
            string metaRobots = collection["selectMetaRobots"];
            string availability_status = collection["selectAvailabilityStatus"];
            DateTime availability_date = DateTime.MinValue;
            DateTime.TryParse(collection["txtAvailabilityDate"], out availability_date);
            string gender = collection["selectGender"];
            string age_group = collection["selectAgeGroup"];
            bool adult_only = Convert.ToBoolean(collection["cbAdultOnly"]);
            decimal unit_pricing_measure = 0;
            decimal.TryParse(collection["txtUnitPricingMeasure"].Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out unit_pricing_measure);
            Int32 unit_pricing_base_measure = 0;
            Int32.TryParse(collection["txtUnitPricingBaseMeasure"].Replace(",", "."), out unit_pricing_base_measure);
            Int32 comparison_unit = Convert.ToInt32(collection["selectComparisonUnit"]);
            string energy_efficiency_class = collection["selectEnergyClass"];
            bool downloadable_files = Convert.ToBoolean(collection["cbDownloadableFiles"]);
            string deliveryTime = collection["txtDeliveryTime"];
            string affiliateLink = collection["txtAffiliateLink"];
            decimal toll_freight_addition = 0;
            decimal.TryParse(collection["txtTollFreight"].Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out toll_freight_addition);
            Int32 valueAddedTaxId = Convert.ToInt32(collection["selectValueAddedTax"]);
            string accountCode = collection["txtAccountCode"];
            string google_category = collection["txtGoogleCategory"];
            bool use_local_images = Convert.ToBoolean(collection["cbLocalImages"]);
            bool use_local_files = Convert.ToBoolean(collection["cbLocalFiles"]);
            DateTime date_added = DateTime.MinValue;
            DateTime.TryParse(collection["txtDateAdded"], out date_added);
            string size_type = collection["selectSizeType"];
            string size_system = collection["selectSizeSystem"];
            bool inactive = Convert.ToBoolean(collection["cbInactive"]);
            
            // Get form values (ProductOptionTypes)
            string[] productOptionTypeIds = collection.GetValues("productOptionTypeId");
            string[] optionTypeIds = collection.GetValues("optionTypeId");
            string[] optionTypeSelectedInput = collection.GetValues("optionTypeSelected");
            string[] optionTypeTitles = collection.GetValues("optionTypeTitle");

            // Get counts
            Int32 optionTypeIdsCount = optionTypeIds != null ? optionTypeIds.Length : 0;
            Int32 optionTypeSelectedInputCount = optionTypeSelectedInput != null ? optionTypeSelectedInput.Length : 0;

            // Get option type selected input
            List<string> optionTypeSelected = new List<string>(optionTypeIdsCount);
            int counter = 0;
            while (counter < optionTypeSelectedInputCount)
            {
                if (optionTypeSelectedInput[counter] == "true")
                {
                    optionTypeSelected.Add("true");
                    counter += 2;
                }
                else
                {
                    optionTypeSelected.Add("false");
                    counter += 1;
                }
            }

            // Get form values (ProductOptions)
            string[] keyOptionTypeIds = collection.GetValues("keyOptionTypeId");
            string[] optionIds = collection.GetValues("optionId");
            string[] optionSelectedInput = collection.GetValues("optionSelected");
            string[] optionTitles = collection.GetValues("optionTitle");
            string[] optionSuffixes = collection.GetValues("optionSuffix");
            string[] optionMpnSuffixes = collection.GetValues("optionMpnSuffix");
            string[] optionPriceAdditions = collection.GetValues("optionPriceAddition");
            string[] optionFreightAdditions = collection.GetValues("optionFreightAddition");

            // Get counts
            Int32 keyOptionTypeIdsCount = keyOptionTypeIds != null ? keyOptionTypeIds.Length : 0;
            Int32 optionSelectedInputCount = optionSelectedInput != null ? optionSelectedInput.Length : 0;

            // Get option selected input
            List<string> optionSelected = new List<string>(keyOptionTypeIdsCount);
            counter = 0;
            while (counter < optionSelectedInputCount)
            {
                if (optionSelectedInput[counter] == "true")
                {
                    optionSelected.Add("true");
                    counter += 2;
                }
                else
                {
                    optionSelected.Add("false");
                    counter += 1;
                }
            }

            // Get the default admin language id
            Int32 adminLanguageId = currentDomain.back_end_language;

            // Get the product
            Product product = Product.GetOneById(productId, adminLanguageId);

            // Get translated texts
            KeyStringList tt = StaticText.GetAll(adminLanguageId, "id", "ASC");

            // Check if the product exists
            if (product == null)
            {
                // Create a new product
                product = new Product();
            }

            // Set values for the product
            product.category_id = categoryId;
            product.title = title;
            product.product_code = productCode;
            product.manufacturer_code = manufacturer_code;
            product.gtin = gtin;
            product.unit_price = price;
            product.unit_freight = freight;
            product.unit_id = unitId;
            product.discount = discount;
            product.mount_time_hours = mountTimeHours;
            product.from_price = from_price;
            product.brand = brand;
            product.supplier_erp_id = supplierErpId;
            product.main_content = description;
            product.extra_content = extra_content;
            product.meta_description = metaDescription;
            product.meta_keywords = metaKeywords;
            product.page_name = pageName;
            product.condition = condition;
            product.variant_image_filename = variant_image_filename;
            product.meta_robots = metaRobots;
            product.gender = gender;
            product.age_group = age_group;
            product.adult_only = adult_only;
            product.unit_pricing_measure = unit_pricing_measure;
            product.unit_pricing_base_measure = unit_pricing_base_measure;
            product.comparison_unit_id = comparison_unit;
            product.size_type = size_type;
            product.size_system = size_system;
            product.energy_efficiency_class = energy_efficiency_class;
            product.downloadable_files = downloadable_files;
            product.delivery_time = deliveryTime;
            product.affiliate_link = affiliateLink;
            product.toll_freight_addition = toll_freight_addition;
            product.value_added_tax_id = valueAddedTaxId;
            product.account_code = accountCode;
            product.google_category = google_category;
            product.use_local_images = use_local_images;
            product.use_local_files = use_local_files;
            product.availability_status = availability_status;
            product.availability_date = AnnytabDataValidation.TruncateDateTime(availability_date);
            product.date_added = AnnytabDataValidation.TruncateDateTime(date_added);
            product.inactive = inactive;

            // Count the product option types
            Int32 optionTypesCount = productOptionTypeIds != null ? productOptionTypeIds.Length : 0;

            // Create the list of product option types
            List<ProductOptionType> productOptionTypes = new List<ProductOptionType>(optionTypesCount);

            // Add all product option types to the list
            for (int i = 0; i < optionTypesCount; i++)
            {
                // Create a product option type
                ProductOptionType productOptionType = new ProductOptionType();
                productOptionType.id = Convert.ToInt32(productOptionTypeIds[i]);
                productOptionType.product_id = productId;
                productOptionType.option_type_id = Convert.ToInt32(optionTypeIds[i]);
                productOptionType.selected = Convert.ToBoolean(optionTypeSelected[i]);
                productOptionType.title = optionTypeTitles[i];
                productOptionType.sort_order = Convert.ToInt16(i);

                // Add the product option type to the list
                productOptionTypes.Add(productOptionType);
            }

            // Create a dictionary for product options
            Dictionary<Int32, List<ProductOption>> productOptions = new Dictionary<Int32, List<ProductOption>>(optionTypesCount);

            // Count product options
            Int32 optionsCount = keyOptionTypeIds != null ? keyOptionTypeIds.Length : 0;

            // Create a new list of product options
            List<ProductOption> listProductOptions = new List<ProductOption>(10);

            // Create a error message
            string errorMessage = string.Empty;

            // Add all product options to the list
            for (int j = 0; j < optionsCount; j++)
            {
                // Create a product option
                Int32 optionTypeId = Convert.ToInt32(keyOptionTypeIds[j]);
                ProductOption productOption = new ProductOption();
                productOption.product_option_type_id = Convert.ToInt32(keyOptionTypeIds[j]);
                productOption.option_id = Convert.ToInt32(optionIds[j]);
                productOption.selected = Convert.ToBoolean(optionSelected[j]);
                productOption.title = optionTitles[j];
                productOption.product_code_suffix = optionSuffixes[j];
                productOption.mpn_suffix = optionMpnSuffixes[j];
                decimal.TryParse(optionPriceAdditions[j].Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out productOption.price_addition);
                decimal.TryParse(optionFreightAdditions[j].Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out productOption.freight_addition);

                // Add the product option to the list
                listProductOptions.Add(productOption);

                // Check for errors
                if (productOption.mpn_suffix.Length > 10)
                {
                    errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("mpn_suffix"), "10") + "<br/>";
                }
                if (productOption.price_addition < 0 || productOption.price_addition > 9999999999.99M)
                {
                    errorMessage += "&#149; " + String.Format(tt.Get("error_field_range"), tt.Get("price_addition"), "9 999 999 999.99") + "<br/>";
                }
                if (productOption.freight_addition < 0 || productOption.freight_addition > 9999999999.99M)
                {
                    errorMessage += "&#149; " + String.Format(tt.Get("error_field_range"), tt.Get("freight_addition"), "9 999 999 999.99") + "<br/>";
                }

                // Check if we should add the list and create a new list
                if ((j + 1) >= optionsCount)
                {
                    // Add the post to the hash table
                    productOptions.Add(optionTypeId, listProductOptions);
                }
                else if (keyOptionTypeIds[j + 1] != keyOptionTypeIds[j])
                {
                    // Add the post to the hash table
                    productOptions.Add(optionTypeId, listProductOptions);

                    // Create a new list
                    listProductOptions = new List<ProductOption>(10);
                }
            }

            // Get a product on page name
            Product productOnPageName = Product.GetOneByPageName(product.page_name, adminLanguageId);

            // Check for errors
            if (productOnPageName != null && product.id != productOnPageName.id)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_language_unique"), tt.Get("page_name")) + "<br/>";
            }
            if (product.page_name == string.Empty)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_required"), tt.Get("page_name")) + "<br/>";
            }
            if (AnnytabDataValidation.CheckPageNameCharacters(product.page_name) == false)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_bad_chars"), tt.Get("page_name")) + "<br/>";
            }
            if(product.category_id == 0)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_select_value"), tt.Get("category").ToLower()) + "<br/>";
            }
            if (product.page_name.Length > 100)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("page_name"), "100") + "<br/>";
            }
            if (product.unit_price < 0 || product.unit_price > 9999999999.99M)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_range"), tt.Get("price"), "9 999 999 999.99") + "<br/>";
            }
            if (product.unit_freight < 0 || product.unit_freight > 9999999999.99M)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_range"), tt.Get("freight"), "9 999 999 999.99") + "<br/>";
            }
            if (product.toll_freight_addition < 0 || product.toll_freight_addition > 9999999999.99M)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_range"), tt.Get("toll_freight_addition"), "9 999 999 999.99") + "<br/>";
            }
            if (product.discount < 0 || product.discount > 9.999M)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_range"), tt.Get("discount"), "9.999") + "<br/>";
            }
            if (product.title.Length > 200)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("title"), "200") + "<br/>";
            }
            if (product.product_code.Length > 20)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("product_code"), "20") + "<br/>";
            }
            if (product.manufacturer_code.Length > 20)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("manufacturer_code"), "20") + "<br/>";
            }
            if (product.gtin.Length > 20)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("gtin").ToUpper(), "20") + "<br/>";
            }
            if (product.brand.Length > 50)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("brand").ToUpper(), "50") + "<br/>";
            }
            if (product.supplier_erp_id.Length > 20)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("supplier_erp_id").ToUpper(), "20") + "<br/>";
            }
            if (product.mount_time_hours < 0 || product.mount_time_hours > 9999.99M)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_range"), tt.Get("mount_time_hours"), "9 999.99") + "<br/>";
            }
            if (product.meta_description.Length > 200)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("meta_description"), "200") + "<br/>";
            }
            if (product.meta_keywords.Length > 200)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("keywords"), "200") + "<br/>";
            }
            if (product.account_code.Length > 10)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("account_code"), "10") + "<br/>";
            }
            if (product.delivery_time.Length > 50)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("delivery_time"), "50") + "<br/>";
            }
            if (product.affiliate_link.Length > 100)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("affiliate_link"), "100") + "<br/>";
            }
            if (product.variant_image_filename.Length > 50)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("variant_image_filename"), "50") + "<br/>";
            }
            if (product.google_category.Length > 250)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("google_category"), "300") + "<br/>";
            }
            if (product.unit_pricing_measure < 0 || product.unit_pricing_measure > 99999.99999M)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_range"), tt.Get("unit_pricing_measure"), "99 999.99999") + "<br/>";
            }

            // Check if there is errors
            if (errorMessage == string.Empty)
            {

                // Check if we should add or update the product
                if (product.id != 0)
                {
                    // Update the product
                    UpdateProduct(product, productOptionTypes, productOptions, adminLanguageId);
                }
                else
                {
                    // Add the product
                    AddProduct(product, productOptionTypes, productOptions, adminLanguageId);
                }

                // Redirect the user to the list
                return Redirect("/admin_products" + returnUrl);
            }
            else
            {
                // Set form values
                ViewBag.ErrorMessage = errorMessage;
                ViewBag.Units = Unit.GetAll(adminLanguageId, "name", "ASC");
                ViewBag.Product = product;
                ViewBag.ProductOptionTypes = productOptionTypes;
                ViewBag.ProductOptions = productOptions;
                ViewBag.TranslatedTexts = tt;
                ViewBag.ReturnUrl = returnUrl;

                // Return the edit view
                return View("edit");
            }

        } // End of the edit method

        // Update images for the product
        // POST: /admin_product/images
        [HttpPost]
        public ActionResult images(FormCollection collection)
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Get query parameters
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

            // Get form values (Product)
            Int32 productId = Convert.ToInt32(collection["txtId"]);
            Int32 languageId = Convert.ToInt32(collection["selectLanguage"]);

            // Get images
            string[] otherImageUrls = collection.GetValues("otherImageUrl");
            HttpPostedFileBase mainImage = null;
            List<HttpPostedFileBase> otherImages = new List<HttpPostedFileBase>(10);

            HttpFileCollectionBase images = Request.Files;
            string[] imageKeys = images.AllKeys;
            for(int i = 0; i < images.Count; i++)
            {
                if (images[i].ContentLength == 0)
                    continue;

                if(imageKeys[i] == "uploadMainImage")
                    mainImage = images[i];
                else
                    otherImages.Add(images[i]);
            }

            // Update images
            UpdateImages(productId, languageId, mainImage, otherImages, otherImageUrls);

            // Redirect the user to the list
            return Redirect("/admin_products" + returnUrl);

        } // End of the images method

        // Add one accessory for the product
        // POST: /admin_products/add_accessory
        [HttpGet]
        public ActionResult add_accessory(Int32 productId = 0, Int32 accessoryId = 0, string returnUrl = "")
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

            // Make sure that the product id or accessory id not is 0
            if(productId == 0 || accessoryId == 0)
            {
                return Redirect("/admin_products" + returnUrl);
            }

            // Check if we can find a saved accessory post
            ProductAccessory accessory = ProductAccessory.GetOneById(productId, accessoryId);

            // Make sure that the accessory does not exist already
            if(accessory == null)
            {
                accessory = new ProductAccessory();
                accessory.product_id = productId;
                accessory.accessory_id = accessoryId;
                ProductAccessory.Add(accessory);
            }

            // Return the accessory view
            return RedirectToAction("accessories", new { id = productId, returnUrl = returnUrl });

        } // End of the add_accessory method

        // Delete a accessory for the product
        // POST: /admin_products/delete_accessory
        [HttpGet]
        public ActionResult delete_accessory(Int32 productId = 0, Int32 accessoryId = 0, string returnUrl = "")
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

            // Make sure that the product id or accessory id not is 0
            if (productId == 0 || accessoryId == 0)
            {
                return Redirect("/admin_products" + returnUrl);
            }

            // Delete the accessory
            ProductAccessory.DeleteOnId(productId, accessoryId);

            // Return the accessory view
            return RedirectToAction("accessories",  new { id = productId, returnUrl = returnUrl });

        } // End of the delete_accessory method

        // Add one bundle item for the product
        // POST: /admin_products/add_bundle_item
        [HttpPost]
        public ActionResult add_bundle_item(FormCollection collection)
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Get query parameters
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

            // Get form data
            Int32 bundleProductId = Convert.ToInt32(collection["hiddenBundleProductId"]);
            Int32 productId = Convert.ToInt32(collection["hiddenProductId"]);
            decimal quantity = 0;
            decimal.TryParse(collection["quantity"].Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out quantity);
            quantity = AnnytabDataValidation.TruncateDecimal(quantity, 0M, 999999.99M);

            // Check if we can find a saved bundle post
            ProductBundle productBundle = ProductBundle.GetOneById(bundleProductId, productId);

            // Make sure that the product bundle does not exist already
            if (productBundle == null)
            {
                // Add a product bundle
                productBundle = new ProductBundle();
                productBundle.bundle_product_id = bundleProductId;
                productBundle.product_id = productId;
                productBundle.quantity = quantity;
                ProductBundle.Add(productBundle);
            }
            else
            {
                // Update a product bundle
                productBundle.quantity = quantity;
                ProductBundle.Update(productBundle);
            }

            // Return the bundle structure view
            return RedirectToAction("bundle_structure", new { id = bundleProductId, returnUrl = returnUrl });

        } // End of the add_bundle_item method

        // Delete a bundle item for the product
        // POST: /admin_products/delete_bundle_item
        [HttpGet]
        public ActionResult delete_bundle_item(Int32 bundleProductId = 0, Int32 productId = 0, string returnUrl = "")
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

            // Make sure that the bundle product id or product id not is 0
            if (bundleProductId == 0 || productId == 0)
            {
                return Redirect("/admin_products" + returnUrl);
            }

            // Delete the product bundle
            ProductBundle.DeleteOnId(bundleProductId, productId);

            // Return the bundle structure view
            return RedirectToAction("bundle_structure", new { id = bundleProductId, returnUrl = returnUrl });

        } // End of the delete_bundle_item method

        // Translate the product
        // POST: /admin_products/translate
        [HttpPost]
        public ActionResult translate(FormCollection collection)
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Get query parameters
            string returnUrl = collection["returnUrl"];
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

            // Get the default admin language
            Int32 adminLanguageId = currentDomain.back_end_language;

            // Get translated texts
            KeyStringList tt = StaticText.GetAll(adminLanguageId, "id", "ASC");

            // Get all the form values
            Int32 translationLanguageId = Convert.ToInt32(collection["selectLanguage"]);
            Int32 productId = Convert.ToInt32(collection["hiddenProductId"]);
            string title = collection["txtTranslatedTitle"];
            string description = collection["txtTranslatedDescription"];
            string extra_content = collection["txtTranslatedExtraContent"];
            string metadescription = collection["txtTranslatedMetadescription"];
            string metakeywords = collection["txtTranslatedMetakeywords"];
            string pagename = collection["txtTranslatedPagename"];
            string deliveryTime = collection["txtTranslatedDeliveryTime"];
            string affiliateLink = collection["txtAffiliateLink"];
            decimal toll_freight_addition = 0;
            decimal.TryParse(collection["txtTollFreight"].Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out toll_freight_addition);
            Int32 valueAddedTaxId = Convert.ToInt32(collection["selectValueAddedTax"]);
            string accountCode = collection["txtAccountCode"];
            string google_category = collection["txtGoogleCategory"];
            bool use_local_images = Convert.ToBoolean(collection["cbLocalImages"]);
            bool use_local_files = Convert.ToBoolean(collection["cbLocalFiles"]);
            string availability_status = collection["selectAvailabilityStatus"];
            DateTime availability_date = DateTime.MinValue;
            DateTime.TryParse(collection["txtAvailabilityDate"], out availability_date);
            string size_type = collection["selectSizeType"];
            string size_system = collection["selectSizeSystem"];
            bool inactive = Convert.ToBoolean(collection["cbInactive"]);

            // Create the translated product
            Product translatedProduct = new Product();
            translatedProduct.id = productId;
            translatedProduct.title = title;
            translatedProduct.main_content = description;
            translatedProduct.extra_content = extra_content;
            translatedProduct.meta_description = metadescription;
            translatedProduct.meta_keywords = metakeywords;
            translatedProduct.page_name = pagename;
            translatedProduct.delivery_time = deliveryTime;
            translatedProduct.affiliate_link = affiliateLink;
            translatedProduct.toll_freight_addition = toll_freight_addition;
            translatedProduct.value_added_tax_id = valueAddedTaxId;
            translatedProduct.account_code = accountCode;
            translatedProduct.google_category = google_category;
            translatedProduct.use_local_images = use_local_images;
            translatedProduct.use_local_files = use_local_files;
            translatedProduct.availability_status = availability_status;
            translatedProduct.availability_date = AnnytabDataValidation.TruncateDateTime(availability_date);
            translatedProduct.size_type = size_type;
            translatedProduct.size_system = size_system;
            translatedProduct.inactive = inactive;

            // Create a error message
            string errorMessage = string.Empty;

            // Get a product on page name
            Product productOnPageName = Product.GetOneByPageName(translatedProduct.page_name, translationLanguageId);

            // Check for errors
            if (productOnPageName != null && translatedProduct.id != productOnPageName.id)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_language_unique"), tt.Get("page_name")) + "<br/>";
            }
            if (translatedProduct.page_name == string.Empty)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_required"), tt.Get("page_name")) + "<br/>";
            }
            if (AnnytabDataValidation.CheckPageNameCharacters(translatedProduct.page_name) == false)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_bad_chars"), tt.Get("page_name")) + "<br/>";
            }
            if (translatedProduct.page_name.Length > 100)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("page_name"), "100") + "<br/>";
            }
            if (translatedProduct.title.Length > 200)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("title"), "200") + "<br/>";
            }
            if (translatedProduct.meta_description.Length > 200)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("meta_description"), "200") + "<br/>";
            }
            if (translatedProduct.meta_keywords.Length > 200)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("keywords"), "200") + "<br/>";
            }
            if (translatedProduct.delivery_time.Length > 50)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("delivery_time"), "50") + "<br/>";
            }
            if (translatedProduct.affiliate_link.Length > 100)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("affiliate_link"), "100") + "<br/>";
            }
            if (translatedProduct.google_category.Length > 300)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("google_category"), "300") + "<br/>";
            }
            if (translatedProduct.toll_freight_addition < 0 || translatedProduct.toll_freight_addition > 9999999999.99M)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_range"), tt.Get("toll_freight_addition"), "9 999 999 999.99") + "<br/>";
            }
            if (translatedProduct.account_code.Length > 10)
            {
                errorMessage += "&#149; " + String.Format(tt.Get("error_field_length"), tt.Get("account_code"), "10") + "<br/>";
            }

            // Check if there is errors
            if (errorMessage == string.Empty)
            {
                // Get the saved product
                Product product = Product.GetOneById(productId, translationLanguageId);

                if (product == null)
                {
                    // Add a new translated product
                    Product.AddLanguagePost(translatedProduct, translationLanguageId);
                }
                else
                {
                    // Update values for saved product
                    product.title = translatedProduct.title;
                    product.main_content = translatedProduct.main_content;
                    product.extra_content = translatedProduct.extra_content;
                    product.meta_description = translatedProduct.meta_description;
                    product.meta_keywords = translatedProduct.meta_keywords;
                    product.page_name = translatedProduct.page_name;
                    product.delivery_time = translatedProduct.delivery_time;
                    product.affiliate_link = translatedProduct.affiliate_link;
                    product.toll_freight_addition = translatedProduct.toll_freight_addition;
                    product.value_added_tax_id = translatedProduct.value_added_tax_id;
                    product.account_code = translatedProduct.account_code;
                    product.google_category = translatedProduct.google_category;
                    product.use_local_images = translatedProduct.use_local_images;
                    product.use_local_files = translatedProduct.use_local_files;
                    product.availability_status = translatedProduct.availability_status;
                    product.availability_date = translatedProduct.availability_date;
                    product.size_type = translatedProduct.size_type;
                    product.size_system = translatedProduct.size_system;
                    product.inactive = translatedProduct.inactive;

                    // Update the product translation
                    Product.UpdateLanguagePost(product, translationLanguageId);
                }

                // Redirect the user to the list
                return Redirect("/admin_products" + returnUrl);
            }
            else
            {
                // Set form values
                ViewBag.LanguageId = translationLanguageId;
                ViewBag.Languages = Language.GetAll(adminLanguageId, "name", "ASC");
                ViewBag.StandardProduct = Product.GetOneById(productId, adminLanguageId);
                ViewBag.TranslatedProduct = translatedProduct;
                ViewBag.ErrorMessage = errorMessage;
                ViewBag.TranslatedTexts = tt;
                ViewBag.ReturnUrl = returnUrl;

                // Return the translate view
                return View("translate");
            }

        } // End of the translate method

        // Delete the product
        // POST: /admin_product/delete
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

            // Get the language id
            int languageId = 0;
            if (Request.Params["lang"] != null)
            {
                Int32.TryParse(Request.Params["lang"], out languageId);
            }

            // Create an error code variable
            Int32 errorCode = 0;

            // Check if we should delete the full post or just the translation
            if (languageId == 0 || languageId == currentDomain.back_end_language)
            {
                // Delete the product and all the posts connected to this product (CASCADE)
                errorCode = Product.DeleteOnId(id);

                // Check if there is an error
                if (errorCode != 0)
                {
                    ViewBag.AdminErrorCode = errorCode;
                    ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
                    return View("index");
                }

                // Delete all files
                DeleteAllFiles(id);

            }
            else
            {
                // Delete the translated post
                errorCode = Product.DeleteLanguagePostOnId(id, languageId);

                // Check if there is an error
                if (errorCode != 0)
                {
                    ViewBag.AdminErrorCode = errorCode;
                    ViewBag.TranslatedTexts = StaticText.GetAll(currentDomain.back_end_language, "id", "ASC");
                    return View("index");
                }

                // Delete all the language files
                DeleteLanguageFiles(id, languageId);
            }

            // Redirect the user to the list
            return Redirect("/admin_products" + returnUrl);

        } // End of the delete method

        // Add downloadable files
        // POST: /admin_products/add_files
        [HttpPost]
        public ActionResult add_files(FormCollection collection)
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Get query parameters
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

            // Get form values
            Int32 languageId = Convert.ToInt32(collection["selectLanguage"]);
            Int32 productId = Convert.ToInt32(collection["txtId"]);
            bool has_downloadable_files = Convert.ToBoolean(collection["cbDownloadableFiles"]);
            DateTime version_date = Convert.ToDateTime(collection["txtVersionDate"]);
            HttpFileCollectionBase uploadedFiles = Request.Files;

            // Create the directory string
            string filesDirectory = "/Content/products/" + (productId / 100).ToString() + "/" + productId.ToString() + "/" + languageId.ToString() + "/dc_files/";

            // Check if the directory exists
            if (System.IO.Directory.Exists(Server.MapPath(filesDirectory)) == false)
            {
                // Create the directory
                System.IO.Directory.CreateDirectory(Server.MapPath(filesDirectory));
            }

            // Save all of the uploaded files
            for (int i = 0; i < uploadedFiles.Count; i++)
            {
                // Set the file path
                string filePath = Server.MapPath(filesDirectory + System.IO.Path.GetFileName(uploadedFiles[i].FileName));

                // Save the uploaded file
                uploadedFiles[i].SaveAs(filePath);

                // Set the last write date of the file
                System.IO.File.SetLastWriteTime(filePath, version_date);
            }

            // Set the has downloadable files boolean for the product
            Product.SetHasDownloadableFiles(productId, has_downloadable_files);

            // Return the files view
            return RedirectToAction("files", new { id = productId, returnUrl = returnUrl, lang = languageId });

        } // End of the add_files method

        // Delete a file
        // POST: /admin_products/delete_file
        [HttpPost]
        public ActionResult delete_file(FormCollection collection)
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Get query parameters
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

            // Get form values
            Int32 productId = Convert.ToInt32(collection["hiddenProductId"]);
            Int32 languageId = Convert.ToInt32(collection["hiddenLanguageId"]);
            string fileName = collection["hiddenFileName"];

            // Create the directory string
            string filesDirectory = "/Content/products/" + (productId / 100).ToString() + "/" + productId.ToString() + "/" + languageId.ToString() + "/dc_files/";

            // Set the filepath
            string filePath = Server.MapPath(filesDirectory + fileName);

            // Check if the file exists
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            // Check if there is any files
            if (System.IO.Directory.GetFiles(Server.MapPath(filesDirectory)).Length == 0)
            {
                Product.SetHasDownloadableFiles(productId, false);
            }

            // Return the files view
            return RedirectToAction("files", new { id = productId, returnUrl = returnUrl, lang = languageId });

        } // End of the delete_file method

        // Update the unit price for all products
        // POST: /admin_product/adjust_unit_prices
        [HttpPost]
        public ActionResult adjust_unit_prices(FormCollection collection)
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Get query parameters
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

            // Get form values
            decimal priceMultiplier = Convert.ToDecimal(collection["priceMultiplier"]) / 100;

            // Update product prices and product option price additions
            Product.UpdateUnitPrices(priceMultiplier);
            ProductOption.UpdatePriceAdditions(priceMultiplier);

            // Redirect the user to the list
            return Redirect("/admin_products" + returnUrl);

        } // End of the adjust_unit_prices method

        // Update the unit freight for all products
        // POST: /admin_product/adjust_unit_freights
        [HttpPost]
        public ActionResult adjust_unit_freights(FormCollection collection)
        {
            // Get the current domain
            Domain currentDomain = Tools.GetCurrentDomain();
            ViewBag.CurrentDomain = currentDomain;

            // Get query parameters
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

            // Get form values
            decimal priceMultiplier = Convert.ToDecimal(collection["priceMultiplier"]) / 100;

            // Update product prices and product option price additions
            Product.UpdateUnitFreights(priceMultiplier);
            ProductOption.UpdateFreightAdditions(priceMultiplier);

            // Redirect the user to the list
            return Redirect("/admin_products" + returnUrl);

        } // End of the adjust_unit_freights method

        #endregion

        #region Helper methods

        /// <summary>
        /// Add the product to the database
        /// </summary>
        /// <param name="product">A reference to a option type</param>
        /// <param name="productOptionTypes">A list of product option types</param>
        /// <param name="productOptions">A dictionary of product options</param>
        /// <param name="languageId">A language id</param>
        private void AddProduct(Product product, List<ProductOptionType> productOptionTypes, Dictionary<Int32, List<ProductOption>> productOptions, Int32 languageId)
        {
            // Save the option type
            long insertId = Product.AddMasterPost(product);
            product.id = Convert.ToInt32(insertId);
            Product.AddLanguagePost(product, languageId);

            // Save all the product options
            for(int i = 0; i < productOptionTypes.Count; i++)
            {
                // Update the product id of the product option type
                productOptionTypes[i].product_id = product.id;

                // Check if the product option type is checked
                if (productOptionTypes[i].selected == true)
                {
                    // Add the product option type
                    Int64 optionTypeId = ProductOptionType.Add(productOptionTypes[i]);
                    productOptionTypes[i].id = Convert.ToInt32(optionTypeId);

                    // Get the list of product options
                    List<ProductOption> listProductOptions = productOptions[productOptionTypes[i].option_type_id];

                    // Loop the list of product options
                    for(int j = 0; j < listProductOptions.Count; j++)
                    {
                        // Update the product option type id
                        listProductOptions[j].product_option_type_id = productOptionTypes[i].id;

                        // Check if the product option is checked
                        if(listProductOptions[j].selected == true)
                        {
                            // Add the product option
                            ProductOption.Add(listProductOptions[j]);
                        }
                    }
                }

            } // End of for(int i = 0; i < productOptionTypes.Count; i++)

        } // End of the AddProduct method

        /// <summary>
        /// Update the product in the database
        /// </summary>
        /// <param name="product">A reference to a product</param>
        /// <param name="productOptionTypes">A list of product option types</param>
        /// <param name="productOptions">A dictionary of product options</param>
        /// <param name="languageId">A language id</param>
        private void UpdateProduct(Product product, List<ProductOptionType> productOptionTypes, Dictionary<Int32, List<ProductOption>> productOptions, Int32 languageId)
        {

            // Update the product
            Product.UpdateMasterPost(product);
            Product.UpdateLanguagePost(product, languageId);

            // Update or add product options
            for (int i = 0; i < productOptionTypes.Count; i++)
            {
                // Check if we should add, update or delete the product option type
                if (productOptionTypes[i].selected == true)
                {
                    // Try to get the the product option type
                    ProductOptionType savedProductOptionType = ProductOptionType.GetOneById(productOptionTypes[i].id, languageId);

                    // Check if we should add or update the product option type
                    if (savedProductOptionType != null)
                    {
                        // Update the product option type
                        ProductOptionType.Update(productOptionTypes[i]);
                    }
                    else
                    {
                        // Add the product option type
                        Int64 optionTypeId = ProductOptionType.Add(productOptionTypes[i]);
                        productOptionTypes[i].id = Convert.ToInt32(optionTypeId);
                    }

                    // Check if we should add, update or delete product options
                    List<ProductOption> listProductOptions = null;

                    // Try to get the list from the dictionary
                    if (productOptions.TryGetValue(productOptionTypes[i].option_type_id, out listProductOptions) == false)
                        continue;

                    // Loop the list of product options
                    for (int j = 0; j < listProductOptions.Count; j++)
                    {

                        // Update the product option id of the product option
                        listProductOptions[j].product_option_type_id = productOptionTypes[i].id;

                        // Check if the product option is checked
                        if (listProductOptions[j].selected == true)
                        {

                            // Get the saved product option
                            ProductOption savedProductOption = ProductOption.GetOneById(listProductOptions[j].product_option_type_id, listProductOptions[j].option_id, languageId);

                            if (savedProductOption != null)
                            {
                                // Update the product option
                                ProductOption.Update(listProductOptions[j]);
                            }
                            else
                            {
                                // Add the product option
                                ProductOption.Add(listProductOptions[j]);
                            }
                        }
                        else
                        {
                            // Delete the product option
                            ProductOption.DeleteOnId(listProductOptions[j].product_option_type_id, listProductOptions[j].option_id);
                        }
                    }
                }
                else
                {
                    // Delete the product option type
                    ProductOptionType.DeleteOnId(productOptionTypes[i].id);
                }

            } // End of for (int i = 0; i < productOptionTypes.Count; i++)

        } // End of the UpdateProduct method

        /// <summary>
        /// Get the main image url for the product
        /// </summary>
        /// <param name="productId">The product id</param>
        /// <param name="languageId">The language id</param>
        /// <returns>An image url as a string</returns>
        public string GetMainImageUrl(Int32 productId, Int32 languageId)
        {
            // Create the string to return
            string imageUrl = "/Content/images/annytab_design/no_image_square.jpg";

            // Create the main image url
            string productMainImageUrl = "/Content/products/" + (productId / 100).ToString() + "/" + productId.ToString() + "/" + languageId.ToString() + "/main_image.jpg";

            // Check if the main image exists
            if (System.IO.File.Exists(Server.MapPath(productMainImageUrl)))
            {
                imageUrl = productMainImageUrl;
            }

            // Return the image url
            return imageUrl;

        } // End of the GetMainImageUrl method

        /// <summary>
        /// Update images
        /// </summary>
        /// <param name="productId">The product id</param>
        /// <param name="languageId">The language id</param>
        /// <param name="mainImage">The posted main image file</param>
        /// <param name="otherImages">A list of posted files for other images</param>
        /// <param name="otherImageUrls">An array of urls to other images</param>
        private void UpdateImages(Int32 productId, Int32 languageId, HttpPostedFileBase mainImage, List<HttpPostedFileBase> otherImages, string[] otherImageUrls)
        {

            // Create directory strings
            string otherImagesDirectory = "/Content/products/" + (productId / 100).ToString() + "/" + productId.ToString() + "/" + languageId.ToString() + "/other_images/";
            string mainImageUrl = "/Content/products/" + (productId / 100).ToString() + "/" + productId.ToString() + "/" + languageId.ToString() + "/main_image.jpg";

            // Check if the directory exists
            if (System.IO.Directory.Exists(Server.MapPath(otherImagesDirectory)) == false)
            {
                // Create the directory
                System.IO.Directory.CreateDirectory(Server.MapPath(otherImagesDirectory));
            }

            // Create an array for urls of saved images
            string[] savedOtherImageUrls = null;

            // Get saved images
            try
            {
                savedOtherImageUrls = System.IO.Directory.GetFiles(Server.MapPath(otherImagesDirectory));
            }
            catch(Exception ex)
            {
                string exMessage = ex.Message;
            }
                 
            // Check for images to delete
            if (savedOtherImageUrls != null)
            {
                // Loop the urls of saved images
                for (int i = 0; i < savedOtherImageUrls.Length; i++)
                {
                    // Create a boolean to indicate if the image should be deleted
                    bool deleteImage = true;

                    // Get the filename of the saved file
                    string savedImageFileName = System.IO.Path.GetFileName(savedOtherImageUrls[i]);

                    // Loop the names of images that exists
                    if (otherImageUrls != null)
                    {
                        for (int j = 0; j < otherImageUrls.Length; j++)
                        {
                            // Get the file name of the other image url
                            string otherImageUrlFileName = System.IO.Path.GetFileName(otherImageUrls[j]);

                            // Check if the file names are equal
                            if (otherImageUrlFileName == savedImageFileName)
                            {
                                deleteImage = false;
                                break;
                            }
                        }
                    }

                    if (deleteImage == true)
                    {
                        // Delete the image
                        System.IO.File.Delete(Server.MapPath(otherImagesDirectory + savedImageFileName));
                    }
                }
            }

            // Save the main image
            if (mainImage != null)
            {
                mainImage.SaveAs(Server.MapPath(mainImageUrl));
            }

            // Save other images
            if (otherImages != null)
            {
                for (int i = 0; i < otherImages.Count; i++)
                {
                    otherImages[i].SaveAs(Server.MapPath(otherImagesDirectory + System.IO.Path.GetFileName(otherImages[i].FileName)));
                }
            }

        } // End of the UpdateImages method

        /// <summary>
        /// Delete all the files for the product
        /// </summary>
        /// <param name="productId">The product id</param>
        private void DeleteAllFiles(Int32 productId)
        {
            // Define the directory url
            string productDirectory = Server.MapPath("/Content/products/" + (productId / 100).ToString() + "/" + productId.ToString());

            // Delete the directory if it exists
            if (System.IO.Directory.Exists(productDirectory))
            {
                System.IO.Directory.Delete(productDirectory, true);
            }

        } // End of the DeleteAllFiles method

        /// <summary>
        /// Delete all the files for the language and the product
        /// </summary>
        /// <param name="productId">The product id</param>
        /// <param name="languageId">The language id</param>
        private void DeleteLanguageFiles(Int32 productId, Int32 languageId)
        {
            // Define the directory url
            string productDirectory = Server.MapPath("/Content/products/" + (productId / 100).ToString() + "/" + productId.ToString() + "/" + languageId.ToString());

            // Delete the directory if it exists
            if (System.IO.Directory.Exists(productDirectory))
            {
                System.IO.Directory.Delete(productDirectory, true);
            }

        } // End of the DeleteLanguageFiles method

        #endregion

    } // End of the class

} // End of the namespace