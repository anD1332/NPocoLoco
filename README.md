Welcome to NPocoLoco!
===================

This readme is currently a work in progress and I am currently working on a nuget package which automatically adds the NPocoLoco config sections

##Nuget Package
[![Nuget version](https://badge.fury.io/nu/NPocoLoco.svg)](http://badge.fury.io/nu/NPocoLoco)

Url : **http://www.nuget.org/packages/NPocoLoco**

Cmd: ```PM> Install-Package NPocoLoco```

##Contents
* [What isNPocoLoco](#what-is-npocoloco)
* [Working Example](#working-example)
* [What is it doing](#what-is-it-doing)
* [Configuration](#configuration)
* [Release Notes](#release-notes)

##What is NPocoLoco
NPocoLoco is a tool, built using NPoco which allows you to create and manage database migrations from one version to another. 

It enables:
* Safe automated deployments of database migrations
* Source controlled migration scripts
* Safe roll back of unsuccessful migrations as all scripts are run inside a transaction which 

###Working Example
Please refer to the NPocoLoco.Tests.Integration project within the solution for a working example of NPocoLoco. In this project there are a couple of scripts.

###What is it doing
[TBC]

###Configuration
The only addition to the configuration is for the NPocoLoco section.

```XML
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <configSections>
    <sectionGroup name="nPocoLocoConfigGroup">
      <section name="nPocoLocoSection"
                type="NPocoLoco.Configuration.NPocoLocoSection, NPocoLoco"
                allowLocation="true"
                allowDefinition="Everywhere" />
    </sectionGroup>
  </configSections>

  <nPocoLocoConfigGroup>
    <nPocoLocoSection connection="NPocoLocoConnectionString" resourcesAssemblyName="NPocoLoco.Tests.Unit"/>
  </nPocoLocoConfigGroup>
</configuration>
```

There are two properties which are required to be setup:
* connection - the name of the connection string within your configuration file you want to run the migrations against
* resourcesAssemblyName - the assembly which contains the SQL scripts

###Release Notes
[TBC]
