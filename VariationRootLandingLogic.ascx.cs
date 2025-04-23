// File: VariationRootLandingLogic.ascx.cs
// Author: Subhabrata Rana
// Description: UserControl logic for SharePoint multilingual redirection based on LCID from user profile and SharePoint variation settings.

using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.UI;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Publishing;
using Microsoft.Office.Server;
using Microsoft.Office.Server.UserProfiles;

namespace VariationNamespace
{
    public partial class PBVariationRootLandingLogic : System.Web.UI.UserControl
    {
        private ReadOnlyCollection<VariationLabel> allLabels = null;
        private Dictionary<string, string> languageToUrl1 = null;
        private string language = string.Empty;
        string sourceUrl = string.Empty;
        string redirectUrl = string.Empty;
        string profileCountry = string.Empty;
        string countryfromList = string.Empty;
        string defaultLanugagefromList = string.Empty;
        string userName = string.Empty;
        UserProfile userProfile = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                try
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    using (SPWeb web = site.RootWeb)
                    {
                        userName = SPContext.Current.Web.CurrentUser.LoginName;
                        SPServiceContext serverContext = SPServiceContext.GetContext(site);
                        if (serverContext != null)
                        {
                            UserProfileManager profileManager = new UserProfileManager(serverContext);
                            if (!string.IsNullOrEmpty(userName))
                                userProfile = profileManager.GetUserProfile(userName);
                            else
                                userProfile = profileManager.GetUserProfile(SPContext.Current.Web.CurrentUser.RawSid);

                            if (userProfile?["Country"].Value != null)
                                profileCountry = userProfile["Country"].Value.ToString();
                        }

                        SPList oList = web.Lists["Variation"];
                        foreach (SPListItem oItem in oList.Items)
                        {
                            countryfromList = Convert.ToString(oItem["Country"]);
                            if (countryfromList == profileCountry)
                            {
                                string[] languageValues = Convert.ToString(oItem["Language"]).Split('(');
                                defaultLanugagefromList = languageValues[1].Split(')')[0];
                                break;
                            }
                        }

                        allLabels = Variations.Current.UserAccessibleLabels;
                        if (allLabels != null && allLabels.Count > 0)
                        {
                            languageToUrl1 = new Dictionary<string, string>();
                            foreach (VariationLabel label in allLabels)
                            {
                                if (label.IsSource) sourceUrl = label.TopWebUrl;

                                if (!languageToUrl1.ContainsKey(label.Locale))
                                {
                                    CultureInfo cultureInfo = CreateCulture(label);
                                    if (cultureInfo != null)
                                        languageToUrl1.Add(Convert.ToString(cultureInfo.LCID), label.TopWebUrl);
                                }
                            }

                            language = defaultLanugagefromList;
                            redirectUrl = sourceUrl;
                            if (!string.IsNullOrEmpty(language) && languageToUrl1.ContainsKey(language))
                                redirectUrl = languageToUrl1[language];

                            SPUtility.Redirect(redirectUrl, SPRedirectFlags.Trusted, HttpContext.Current);
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.Controls.Add(new LiteralControl(ex.Message));
                }
            });
        }

        private CultureInfo CreateCulture(VariationLabel label)
        {
            if (label != null && !string.IsNullOrEmpty(label.Locale))
                return new CultureInfo(Convert.ToInt32(label.Locale), false);
            return null;
        }
    }
}
