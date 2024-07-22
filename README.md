# Xperience by Kentico Google Maps

[![Kentico Labs](https://img.shields.io/badge/Kentico_Labs-grey?labelColor=orange&logo=data:image/svg+xml;base64,PHN2ZyBjbGFzcz0ic3ZnLWljb24iIHN0eWxlPSJ3aWR0aDogMWVtOyBoZWlnaHQ6IDFlbTt2ZXJ0aWNhbC1hbGlnbjogbWlkZGxlO2ZpbGw6IGN1cnJlbnRDb2xvcjtvdmVyZmxvdzogaGlkZGVuOyIgdmlld0JveD0iMCAwIDEwMjQgMTAyNCIgdmVyc2lvbj0iMS4xIiB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciPjxwYXRoIGQ9Ik05NTYuMjg4IDgwNC40OEw2NDAgMjc3LjQ0VjY0aDMyYzE3LjYgMCAzMi0xNC40IDMyLTMycy0xNC40LTMyLTMyLTMyaC0zMjBjLTE3LjYgMC0zMiAxNC40LTMyIDMyczE0LjQgMzIgMzIgMzJIMzg0djIxMy40NEw2Ny43MTIgODA0LjQ4Qy00LjczNiA5MjUuMTg0IDUxLjIgMTAyNCAxOTIgMTAyNGg2NDBjMTQwLjggMCAxOTYuNzM2LTk4Ljc1MiAxMjQuMjg4LTIxOS41MnpNMjQxLjAyNCA2NDBMNDQ4IDI5NS4wNFY2NGgxMjh2MjMxLjA0TDc4Mi45NzYgNjQwSDI0MS4wMjR6IiAgLz48L3N2Zz4=)](https://github.com/Kentico/.github/blob/main/SUPPORT.md#labs-limited-support) [![CI: Build and Test](https://github.com/Kentico/xperience-by-kentico-google-maps/actions/workflows/ci.yml/badge.svg?branch=main)](https://github.com/Kentico/xperience-by-kentico-google-maps/actions/workflows/ci.yml)

## Description

Xperience by Kentico Google Maps integration allows users to add an address input field with validation and autocomplete to their websites via a new form component using the [Forms](https://docs.kentico.com/business-users/digital-marketing/forms) application.

## Screenshots

![Google Maps autocomplete on live site](/images/GoogleAutocomplete.png)

## Library Version Matrix

| Xperience Version | Library Version |
| ----------------- | --------------- |
| >= 29.0.3         | 1.0.0           |

### Dependencies

- [ASP.NET Core 8.0](https://dotnet.microsoft.com/en-us/download)
- [Xperience by Kentico](https://docs.xperience.io/xp/changelog)

## Package Installation

Add the package to your application using the .NET CLI. Run the following command from the root of your Xperience project.

```powershell
dotnet add package Kentico.Xperience.GoogleMaps
```

## Quick Start

The integration uses Google Cloud console APIs, which must be configured together with your Xperience project when setting up the integration.

### Google Cloud Setup

1. Sign in to [Google Cloud console](https://console.cloud.google.com/). You will need to create a Google account if you don't already have one.
2. [Create](https://developers.google.com/workspace/guides/create-project) a new Google Cloud project.
3. [Enable](https://cloud.google.com/endpoints/docs/openapi/enable-api) the following APIs: 
    - [Geocoding API](https://console.cloud.google.com/apis/library/geocoding-backend.googleapis.com)
    - [Places API](https://console.cloud.google.com/apis/library/places-backend.googleapis.com)
    - [Maps API](https://console.cloud.google.com/apis/library/maps-backend.googleapis.com)
    - [Address Validation API](https://console.cloud.google.com/apis/library/addressvalidation.googleapis.com)
4. [Create](https://cloud.google.com/docs/authentication/api-keys#create) an API key for your project. You will later use this key when configuring the Google Maps integration in your Xperience project.

### Integration Setup

1. Add the `Kentico.Xperience.GoogleMaps` NuGet package to your Xperience project.

   ```powershell
   dotnet add package Kentico.Xperience.GoogleMaps
   ```

2. In `Program.cs`, call the `AddXperienceGoogleMaps` method with the `builder.Configuration` parameter when registering services into the application's service container. The method must be called after `AddKentico`.

   ```csharp
	WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
	builder.Services.AddKentico();
	builder.Services.AddXperienceGoogleMaps(builder.Configuration);
	```

3. Add the following configuration keys to your application's configuration providers (`appsettings.json` by default). Use the API you generated for your Google Cloud console project in the previous steps.

	```json
	"CMSXperienceGoogleMaps": {
	  "APIKey": "<Google Maps API key>"
	}
	```

4. The integration provides a `<google-maps />` Razor [Tag Helper](https://learn.microsoft.com/en-us/aspnet/core/mvc/views/tag-helpers/intro?view=aspnetcore-8.0) that includes the scripts and styles necessary for the integration to function. Include the Tag Helper to a suitable location in your Razor view hierarchy using the `@addTagHelper` directive. Add the Tag Helper to your top-level `_ViewImports.cshtml` file to make it available everywhere.

	```csharp
	@using Kentico.Xperience.GoogleMaps
	@addTagHelper *, Kentico.Xperience.GoogleMaps
	```

5. Place the `<google-maps />` Tag Helper in the `<head>` tag of your page's HTML structure as defined by your Razor layout hierarchy. The Tag Helper must be placed everywhere where forms that use the address autocomplete form component are rendered.

	```csharp
	<google-maps />
	```

## Customization of Autocomplete panel

To customize the appearance of the autocomplete panel, you can use the following CSS classes:
- `xperience-address-dropdown` - the main container of the autocomplete dropdown
- `xperience-address-dropdown-item` - the container of each item in the dropdown
- `xperience-address-dropdown-item:hover` - the hover effect of the dropdown item
- `xperience-address-dropdown-item.active` - the active item in the dropdown
- `xperience-address-dropdown-item-logo` - the container of the Google Maps logo dropdown item

Keep in mind [Google's usage policies](https://developers.google.com/maps/documentation/places/web-service/policies).

## Contributing

To see the guidelines for Contributing to Kentico open source software, please see [Kentico's `CONTRIBUTING.md`](https://github.com/Kentico/.github/blob/main/CONTRIBUTING.md) for more information and follow the [Kentico's `CODE_OF_CONDUCT`](https://github.com/Kentico/.github/blob/main/CODE_OF_CONDUCT.md).

Instructions and technical details for contributing to **this** project can be found in [Contributing Setup](./docs/Contributing-Setup.md).

## License

Distributed under the MIT License. See [`LICENSE.md`](./LICENSE.md) for more information.

## Support

[![Kentico Labs](https://img.shields.io/badge/Kentico_Labs-grey?labelColor=orange&logo=data:image/svg+xml;base64,PHN2ZyBjbGFzcz0ic3ZnLWljb24iIHN0eWxlPSJ3aWR0aDogMWVtOyBoZWlnaHQ6IDFlbTt2ZXJ0aWNhbC1hbGlnbjogbWlkZGxlO2ZpbGw6IGN1cnJlbnRDb2xvcjtvdmVyZmxvdzogaGlkZGVuOyIgdmlld0JveD0iMCAwIDEwMjQgMTAyNCIgdmVyc2lvbj0iMS4xIiB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciPjxwYXRoIGQ9Ik05NTYuMjg4IDgwNC40OEw2NDAgMjc3LjQ0VjY0aDMyYzE3LjYgMCAzMi0xNC40IDMyLTMycy0xNC40LTMyLTMyLTMyaC0zMjBjLTE3LjYgMC0zMiAxNC40LTMyIDMyczE0LjQgMzIgMzIgMzJIMzg0djIxMy40NEw2Ny43MTIgODA0LjQ4Qy00LjczNiA5MjUuMTg0IDUxLjIgMTAyNCAxOTIgMTAyNGg2NDBjMTQwLjggMCAxOTYuNzM2LTk4Ljc1MiAxMjQuMjg4LTIxOS41MnpNMjQxLjAyNCA2NDBMNDQ4IDI5NS4wNFY2NGgxMjh2MjMxLjA0TDc4Mi45NzYgNjQwSDI0MS4wMjR6IiAgLz48L3N2Zz4=)](https://github.com/Kentico/.github/blob/main/SUPPORT.md#labs-limited-support)

This project has **Kentico Labs limited support**.

See [`SUPPORT.md`](https://github.com/Kentico/.github/blob/main/SUPPORT.md#full-support) for more information.

For any security issues see [`SECURITY.md`](https://github.com/Kentico/.github/blob/main/SECURITY.md).
