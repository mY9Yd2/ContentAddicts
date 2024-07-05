# Contributing

## Table of contents

- [Setup user-secrets](#setup-user-secrets)

## Setup user-secrets

Using the `dotnet user-secrets` command. [See the docs](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets) or `--help`.  
The ids can be found in the *.csproj files.

ContentAddicts.Api:  
`DefaultConnectionString = server=localhost;user=;password=;database=ContentAddicts`

ContentAddicts.IntegrationTests:  
`TestIntegrationDefaultConnectionString = server=localhost;user=;password=;database=ContentAddictsTestIntegration`

ContentAddicts.UnitTests:  
`TestDefaultConnectionString = server=localhost;user=;password=;database=ContentAddictsTest`
