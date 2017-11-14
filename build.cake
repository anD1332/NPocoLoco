
var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var version = EnvironmentVariable("PACKVERSION") ?? "1.0.0";

var buildDir = Directory("./src/NPocoLoco/bin") + Directory(configuration);

Task("Clean")
    .Does(() => {
        CleanDirectory(buildDir);
        CleanDirectory("./artefacts");
        CleanDirectory("./src/packages");
        CleanDirectory("./nugetpackage");
    });

Task("Restore")
    .IsDependentOn("Clean")
    .Does(() => {
        NuGetRestore("./src/NPocoLoco.sln");
    });

Task("Build")
    .IsDependentOn("Restore")
    .Does(() => {
        DotNetBuild("./src/NPocoLoco.sln", settings =>
            settings.SetConfiguration(configuration) 
            .WithTarget("Build")
            .WithProperty("TreatWarningsAsErrors", "true"));
    });



Task("Default")
    .IsDependentOn("Build");

RunTarget(target);