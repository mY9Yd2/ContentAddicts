# Contributing

## Table of contents

- [Setup user-secrets](#setup-user-secrets)
- [GitHub actions](#github-actions)

## Setup user-secrets

Using the `dotnet user-secrets` command. [See the docs](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets) or `--help`.  
The `UserSecretsId` can be found in the *.csproj files.

ContentAddicts.Api:

```text
dotnet user-secrets set --project ContentAddicts.Api/ "DefaultConnectionString" "server=localhost;user=;password=;database=ContentAddicts"
```

ContentAddicts.IntegrationTests:

```text
dotnet user-secrets set --project ContentAddicts.IntegrationTests/ "IntegrationTestsConnectionString" "server=localhost;user=;password=;database=ContentAddictsIntegrationTests"
```

ContentAddicts.UnitTests:

```text
dotnet user-secrets set --project ContentAddicts.UnitTests/ "UnitTestsConnectionString" "server=localhost;user=;password=;database=ContentAddictsUnitTests"
```

## GitHub actions

Run your GitHub Actions locally with [act](https://github.com/nektos/act)

Example:

```text
act push --var-file --secret-file
```

In the repository root:

- `.secrets` file

```text
MARIADB_ROOT_PASSWORD=""
IntegrationTestsConnectionString="server=;port=3306;user=;password=;database=ContentAddictsIntegrationTests"
UnitTestsConnectionString="server=;port=3306;user=;password=;database=ContentAddictsUnitTests"
```

- `.vars` file

```text
MARIADB_HOST_PORT=3306
```
