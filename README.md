# Xperience by Kentico Google Maps

[![Kentico Labs](https://img.shields.io/badge/Kentico_Labs-grey?labelColor=orange&logo=data:image/svg+xml;base64,PHN2ZyBjbGFzcz0ic3ZnLWljb24iIHN0eWxlPSJ3aWR0aDogMWVtOyBoZWlnaHQ6IDFlbTt2ZXJ0aWNhbC1hbGlnbjogbWlkZGxlO2ZpbGw6IGN1cnJlbnRDb2xvcjtvdmVyZmxvdzogaGlkZGVuOyIgdmlld0JveD0iMCAwIDEwMjQgMTAyNCIgdmVyc2lvbj0iMS4xIiB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciPjxwYXRoIGQ9Ik05NTYuMjg4IDgwNC40OEw2NDAgMjc3LjQ0VjY0aDMyYzE3LjYgMCAzMi0xNC40IDMyLTMycy0xNC40LTMyLTMyLTMyaC0zMjBjLTE3LjYgMC0zMiAxNC40LTMyIDMyczE0LjQgMzIgMzIgMzJIMzg0djIxMy40NEw2Ny43MTIgODA0LjQ4Qy00LjczNiA5MjUuMTg0IDUxLjIgMTAyNCAxOTIgMTAyNGg2NDBjMTQwLjggMCAxOTYuNzM2LTk4Ljc1MiAxMjQuMjg4LTIxOS41MnpNMjQxLjAyNCA2NDBMNDQ4IDI5NS4wNFY2NGgxMjh2MjMxLjA0TDc4Mi45NzYgNjQwSDI0MS4wMjR6IiAgLz48L3N2Zz4=)](https://github.com/Kentico/.github/blob/main/SUPPORT.md#labs-limited-support) [![CI: Build and Test](https://github.com/Kentico/xperience-by-kentico-google-maps/actions/workflows/ci.yml/badge.svg?branch=main)](https://github.com/Kentico/xperience-by-kentico-google-maps/actions/workflows/ci.yml)

## Description

Xperience by Kentico Google Maps integration allows users to add Address field with validation and autocomplete to their websites using Forms application.

## Screenshots

![Google Maps autocomplete on live site](/images/GoogleAutocomplete.png)

## Library Version Matrix

| Xperience Version | Library Version |
| ----------------- | --------------- |
| >= 29.0.0         | 1.0.0           |

### Dependencies

- [ASP.NET Core 8.0](https://dotnet.microsoft.com/en-us/download)
- [Xperience by Kentico](https://docs.xperience.io/xp/changelog)

## Package Installation

Add the package to your application using the .NET CLI

```powershell
dotnet add package Kentico.Xperience.GoogleMaps
```

## Quick Start

### Google Cloud Setup

1. Log into [Google Cloud](https://console.cloud.google.com/)

2. Create a new project
 
3. Enable the following APIs: [Geocoding API](https://console.cloud.google.com/apis/library/geocoding-backend.googleapis.com), [Places API](https://console.cloud.google.com/apis/library/places-backend.googleapis.com), [Maps API](https://console.cloud.google.com/apis/library/maps-backend.googleapis.com), [Address Validation API](https://console.cloud.google.com/apis/library/addressvalidation.googleapis.com)

4. Copy the [API key](https://console.cloud.google.com/apis/credentials) and paste it into `appsettings.json` file later

### Integration Setup

1. Include `Kentico.Xperience.GoogleMaps` project in the solution.

   ```powershell
   dotnet add package Kentico.Xperience.GoogleMaps
   ```

2. In `Program.cs` file add the following last line:

   ```csharp
	WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
	builder.Services.AddKentico();
	builder.Services.AddXperienceGoogleMaps(builder.Configuration);
	```

3. In the `appsettings.json` file, add the following lines:

	```json
	"CMSXperienceGoogleMaps": {
	  "APIKey": "<Google Maps API key>"
	}
	```

4. In the` _ViewImports.cshtml` file, place the Tag Helper to a suitable location

	```csharp
	@using Kentico.Xperience.GoogleMaps
	@addTagHelper *, Kentico.Xperience.GoogleMaps
	```

5. In the website layout file (for example: `_DancingGoutLayout.cshtml`), place the Tag Helper inside the head tag:

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

Remember to not break any of [Google's usage policies](https://developers.google.com/maps/documentation/places/web-service/policies).

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
