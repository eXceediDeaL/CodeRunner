# CodeRunner.Test.Host

This fork of [CodeRunner](https://github.com/CodeRunner-SD/CodeRunner) maintains a package for testing extension with real environment.

|Item|Status|
|-|-|
|Repository|[![issue](https://img.shields.io/github/issues/eXceediDeaL/CodeRunner.svg)](https://github.com/eXceediDeaL/CodeRunner/issues/) [![pull requests](https://img.shields.io/github/issues-pr/eXceediDeaL/CodeRunner.svg)](https://github.com/eXceediDeaL/CodeRunner/pulls/)|
|Dependencies|[![dependencies](https://img.shields.io/librariesio/github/eXceediDeaL/CodeRunner.svg)](https://libraries.io/github/eXceediDeaL/CodeRunner)|
|Build|![github](https://github.com/eXceediDeaL/CodeRunner/workflows/CI-CD/badge.svg)|
|Package|[![myget](https://img.shields.io/myget/stardustdl/v/CodeRunner.Test.Host?label=myget)](https://www.myget.org/feed/stardustdl/package/nuget/CodeRunner.Test.Host) [![myget](https://img.shields.io/myget/stardustdl/dt/CodeRunner.Test.Host)](https://www.myget.org/feed/stardustdl/package/nuget/CodeRunner.Test.Host)|

## Usage

1. Create a new console project.
2. Reference the package `CodeRunner.Test.Host`.
```sh
dotnet add package CodeRunner.Test.Host --version 0.0.1-pre --source https://www.myget.org/F/stardustdl/api/v3/index.json
```
3. Add the extension assembly to `CodeRunner.Test.ExtensionDI.ExtensionAssemblies`.
4. Invoke `CodeRunner.Program.Main` at entrypoint.
