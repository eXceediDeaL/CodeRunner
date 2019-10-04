# CodeRunner

[![](https://img.shields.io/github/stars/StardustDL/CodeRunner.svg?style=social&label=Stars)](https://github.com/StardustDL/CodeRunner) [![](https://img.shields.io/github/forks/StardustDL/CodeRunner.svg?style=social&label=Fork)](https://github.com/StardustDL/CodeRunner) [![](https://img.shields.io/github/license/StardustDL/CodeRunner.svg)](https://github.com/StardustDL/CodeRunner/blob/master/LICENSE)

A CLI tool to run code. Inspired by [edl-cr](https://github.com/eXceediDeaL/edl-coderunner).

|Item|Status|
|-|-|
|Repository|[![issue](https://img.shields.io/github/issues/CodeRunner-SD/CodeRunner.svg)](https://github.com/CodeRunner-SD/CodeRunner/issues/) [![pull requests](https://img.shields.io/github/issues-pr/CodeRunner-SD/CodeRunner.svg)](https://github.com/CodeRunner-SD/CodeRunner/pulls/)|
|Dependencies|[![dependencies](https://img.shields.io/librariesio/github/CodeRunner-SD/CodeRunner.svg)](https://libraries.io/github/CodeRunner-SD/CodeRunner)|
|Build|![github](https://github.com/CodeRunner-SD/CodeRunner/workflows/CI-CD/badge.svg)|
|Coverage|[![master](https://img.shields.io/codecov/c/github/CodeRunner-SD/CodeRunner/master.svg?label=master)](https://codecov.io/gh/CodeRunner-SD/CodeRunner) [![dev](https://img.shields.io/codecov/c/github/CodeRunner-SD/CodeRunner/dev.svg?label=dev)](https://codecov.io/gh/CodeRunner-SD/CodeRunner)|
|Package|[![myget](https://img.shields.io/myget/stardustdl/v/CodeRunner?label=myget)](https://www.myget.org/feed/stardustdl/package/nuget/CodeRunner) [![myget](https://img.shields.io/myget/stardustdl/dt/CodeRunner)](https://www.myget.org/feed/stardustdl/package/nuget/CodeRunner)|

## Install

### .NET Tool

CodeRunner is packed as a .NET Tool, and is pushed to MyGet. Use the following command to install it as global tool:

```sh
dotnet tool install -g CodeRunner --version 0.0.1-pre --add-source https://www.myget.org/F/stardustdl/api/v3/index.json
```

### Binary Files

See the latest action CI-CD, and download the artifacts `built` for your OS.

## CLI Mode

|Option|Description|
|-|-|
|`-d --dir`|Set working directory|
|`-c --command`|Execute command just like in interactive mode|
|`-V --version`|Show CR version|
|`-v --verbose`|Enable `DEBUG` level for logging|

## Interactive Mode

If you don't use `--command` options, CR will run in interactive mode.

### Initialize

```sh
> init
```

Initialize CR data. It will create a directory named `.cr` in current directory.

If you want to clear CR data, use this command:

```sh
> init --delete
```

### Create and Run

Create a new item:

```sh
> new cpp a
```

It will use templates in `.cr/templates/` to create item.

If you want to set current work-item with an existed file, use this:

```sh
> now -- type=file target=a.cpp
```

Then use `run` command to run code.

```sh
# run a.cpp
a.cpp> run cpp
```

### Use Directory

Not only use files, you can also use directories to create a unique environment for codes.

```sh
# Create a new directory env
> new dir a

# Set a directory env for current
> now -- type=dir target=a

# Run
@a> run dir
```

For `run` command, it will use the command list in `settings.json` in the directory. You can write your own commands in it. And these command works in the directory of current work-item.

### Debug

When you meet some errors, for example, CR data loading failing, use `debug` command to get some information. This is also a useful tool when you create an issue.

### Commands

|Command|Description|
|-|-|
|`init [--delete]`|Initialize or delete CR data|
|`clear`|Clear screen|
|`new [Template] [Name]`|Create new item by template|
|`now [--clear]`|Change current work-item|
|`run [Operation]`|Run operation|
|`template [list add remove]`|Manage templates|
|`operation [list add remove]`|Manage operations|
|`debug`|Show debug data|
|`exit`|Exit CR|
|`--help`|Get help|
|`--version`|Get version|

# Config

The config files is at `.cr/`

## templates/

This directory contains templates.

## operations/

This directory contains operations.
