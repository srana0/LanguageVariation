# SharePoint MUI Redirection Based on LCID

This SharePoint component handles multilingual redirection by dynamically selecting the target language site based on the user's profile country (via UserProfile Service) and matching it against predefined LCID mappings in a SharePoint list.

## Features

- Automatically redirects users to language-specific SharePoint variations.
- Uses user profile "Country" field to determine language mapping.
- Pulls variation configuration from a custom SharePoint list.
- Supports LCID-based culture resolution and redirection via SharePoint’s `SPUtility.Redirect`.
- Developed as an ASCX UserControl.

## Technologies Used

- C# (.NET Framework)
- SharePoint Server Object Model
- User Profiles & Variations API
- ASP.NET Web Forms
- Web.config for legacy AJAX and SharePoint assemblies

## File Structure

- `PBVariationRootLandingLogic.ascx.cs`: Main logic file
- `Web.config`: Required legacy ASP.NET and SharePoint references

## Setup Instructions

1. Deploy the `PBVariationRootLandingLogic.ascx` and code-behind file into your SharePoint 2010/2013 solution.
2. Register the UserControl in your master page or page layout.
3. Ensure the SharePoint list **Variation** contains `Country` and `Language` fields in the expected format.
4. Configure the correct user profile service application.
5. Deploy and test in an elevated-privilege context.

---

## Author

Developed by **Subhabrata Rana**  
Date: 2025
