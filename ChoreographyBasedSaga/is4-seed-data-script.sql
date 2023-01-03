--order api 
INSERT INTO public."ApiScopes"
("Enabled", "Name", "DisplayName", "Description", "Required", "Emphasize", "ShowInDiscoveryDocument")
VALUES (true, 'secure-order-api', 'Grant for accessing Order Api', null, false, false, true);

INSERT INTO public."ApiResources"
("Enabled", "Name", "DisplayName", "Description", "AllowedAccessTokenSigningAlgorithms", "ShowInDiscoveryDocument", "Created", "Updated", "LastAccessed", "NonEditable")
VALUES (true,'order-api',null, null, null, true, NOW(), null, null, false);

INSERT INTO public."ApiResourceScopes"
("Scope", "ApiResourceId")
VALUES ('secure-order-api', 3);
--stock api
INSERT INTO public."ApiScopes"
("Enabled", "Name", "DisplayName", "Description", "Required", "Emphasize", "ShowInDiscoveryDocument")
VALUES (true, 'secure-stock-api', 'Grant for accessing Stock Api', null, false, false, true);

INSERT INTO public."ApiResources"
("Enabled", "Name", "DisplayName", "Description", "AllowedAccessTokenSigningAlgorithms", "ShowInDiscoveryDocument", "Created", "Updated", "LastAccessed", "NonEditable")
VALUES (true,'stock-api',null, null, null, true, NOW(), null, null, false);

INSERT INTO public."ApiResourceScopes"
("Scope", "ApiResourceId")
VALUES ('secure-stock-api', 4);
--payment api
INSERT INTO public."ApiScopes"
("Enabled", "Name", "DisplayName", "Description", "Required", "Emphasize", "ShowInDiscoveryDocument")
VALUES (true, 'secure-payment-api', 'Grant for accessing Payment Api', null, false, false, true);

INSERT INTO public."ApiResources"
("Enabled", "Name", "DisplayName", "Description", "AllowedAccessTokenSigningAlgorithms", "ShowInDiscoveryDocument", "Created", "Updated", "LastAccessed", "NonEditable")
VALUES (true,'payment-api',null, null, null, true, NOW(), null, null, false);

INSERT INTO public."ApiResourceScopes"
("Scope", "ApiResourceId")
VALUES ('secure-payment-api', 5);


INSERT INTO public."Clients"
("Enabled", "ClientId", "ProtocolType", "RequireClientSecret", "ClientName", "Description", "ClientUri", "LogoUri", "RequireConsent", "AllowRememberConsent", "AlwaysIncludeUserClaimsInIdToken", "RequirePkce", "AllowPlainTextPkce", "RequireRequestObject", "AllowAccessTokensViaBrowser", "FrontChannelLogoutUri", "FrontChannelLogoutSessionRequired", "BackChannelLogoutUri", "BackChannelLogoutSessionRequired", "AllowOfflineAccess", "IdentityTokenLifetime", "AllowedIdentityTokenSigningAlgorithms", "AccessTokenLifetime", "AuthorizationCodeLifetime", "ConsentLifetime", "AbsoluteRefreshTokenLifetime", "SlidingRefreshTokenLifetime", "RefreshTokenUsage", "UpdateAccessTokenClaimsOnRefresh", "RefreshTokenExpiration", "AccessTokenType", "EnableLocalLogin", "IncludeJwtId", "AlwaysSendClientClaims", "ClientClaimsPrefix", "PairWiseSubjectSalt", "Created", "Updated", "LastAccessed", "UserSsoLifetime", "UserCodeType", "DeviceCodeLifetime", "NonEditable")
VALUES (true, 'poc-client', 'oidc', true, 'M2M Client', null, null, null, false, true, false, true, false, false, false, null, true, null, true, true, 300, null, 30, 300, null, 2592000, 1296000, 1, false, 1, 0, true, true, false, 'client_', null, now(), null, null, null, null, 300, false);

INSERT INTO public."ClientSecrets"
("ClientId", "Description", "Value", "Expiration", "Type", "Created")
VALUES (6, null, 'c5uGSdlMmMeRdw5PbDs70Mq8rbSXvrG6JnpsuHX7nbI=', null, 'SharedSecret', Now());

INSERT INTO public."ClientScopes"
("Scope", "ClientId")
VALUES ('secure-order-api', 6);

INSERT INTO public."ClientScopes"
("Scope", "ClientId")
VALUES ('secure-stock-api', 6);

INSERT INTO public."ClientScopes"
("Scope", "ClientId")
VALUES ('secure-payment-api', 6);

INSERT INTO public."ClientGrantTypes"
("GrantType", "ClientId")
VALUES ('client_credentials', 6);