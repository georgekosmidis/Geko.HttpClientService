# Changelog
All notable changes to this project will be documented in this file.

## [3.0.0] - 2021-12-21
### Changes
Project renamed to Geko.HttpClientService
Project upgraded to .NET 6

## [2.3.0] - 2020-03-19
### Added
- Options (in `appsettings.json`) for the header colleration Id. 
### Changed
- `CompleteSample` now includes these options.


## [2.2.2] - 2020-03-16
### Changes
- `HeadersSet` marked as obsolete.

## [2.2.1] - 2020-03-11
### Fixed
- Backward compatibility bug, with random configuration section names.

## [2.2.0] - 2020-03-11
### Added
- Support for `PasswordTokenRequest`.
- `GetTokenResponse()` added in `HttpClientService`
- Sample for `TypeContent`
### Changes
- `CompleteSample` extended to include `PasswordOptions`.
- `HttpHeaders` properties names changed to be grouped in intellisense popups.
- `Scopes` changed to `Scope` to match `IdentityModel`

## [2.1.0] - 2020-03-06
### Added
- `TypeContent()` added for customizing encoding and media-type of serialized complex types

## [2.0.0] - 2020-03-05
### Added
- `HttpClientServiceFactory`, singleton pattern added for use in desktop apps.
### Changes
- `HttpClientService` refactored to clear the clutter.
- `HttpHeaders` properties names changed to be grouped in intellisense popups.
- `Scopes` changed to `Scope` to match `IdentityModel`

## [1.1.0] - 2020-03-03
### Fixed
- Socket exhaustion issue with `HttpClient`

## [1.0.1] - 2020-03-01
### Added
- Azure pipeline for deployment

## [1.0.0] - 2020-03-01
### Added
- Azure pipelines defined for test and build
- A feature and a complete sample created
- README.md was extended

## [0.0.1] - 2020-03-01
- Initial commit