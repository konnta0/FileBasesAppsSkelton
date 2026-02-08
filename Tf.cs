#!/usr/bin/env -S dotnet run
#:package ConsoleAppFramework@5.7.13
#:package ProcessX@1.5.6

using ConsoleAppFramework;
using Cysharp.Diagnostics;
using Zx;

var app = ConsoleApp.Create();
app.Add<TerraformCommands>();
await app.RunAsync(args);

/// <summary>
/// Terraform CLI wrapper for managing SigNoz dashboards
/// </summary>
public sealed class TerraformCommands
{
    /// <summary>
    /// Initialize Terraform working directory
    /// </summary>
    public async Task Init()
    {
        Console.WriteLine("🚀 Initializing Terraform...");
        await "terraform init";
        Console.WriteLine("✅ Terraform initialized successfully");
    }

    /// <summary>
    /// Validate Terraform configuration files
    /// </summary>
    public async Task Validate()
    {
        Console.WriteLine("🔍 Validating Terraform configuration...");
        await "terraform validate";
        Console.WriteLine("✅ Configuration is valid");
    }

    /// <summary>
    /// Format Terraform configuration files
    /// </summary>
    /// <param name="r">-r, Format files in subdirectories recursively</param>
    /// <param name="check">-check, Check if files are formatted without modifying</param>
    public async Task Fmt(bool r = false, bool check = false)
    {
        Console.WriteLine("📝 Formatting Terraform files...");

        var args = "terraform fmt";
        if (r) args += " -recursive";
        if (check) args += " -check";

        await args;
        Console.WriteLine("✅ Formatting complete");
    }

    /// <summary>
    /// Generate and show execution plan
    /// </summary>
    /// <param name="out">-o, Save plan to file</param>
    public async Task Plan(string? @out = null)
    {
        Console.WriteLine("📋 Generating Terraform plan...");

        var args = "terraform plan";
        if (!string.IsNullOrEmpty(@out))
        {
            args += $" -out={@out}";
        }

        await args;
        Console.WriteLine("✅ Plan generated successfully");
    }

    /// <summary>
    /// Apply Terraform configuration
    /// </summary>
    /// <param name="autoApprove">-auto-approve, Skip interactive approval</param>
    /// <param name="planFile">-plan, Apply a saved plan file</param>
    public async Task Apply(bool autoApprove = false, string? planFile = null)
    {
        Console.WriteLine("🔧 Applying Terraform configuration...");

        var args = "terraform apply";
        if (autoApprove) args += " -auto-approve";
        if (!string.IsNullOrEmpty(planFile))
        {
            args += $" {planFile}";
        }

        await ProcessX.StartAsync(args).WriteLineAllAsync();

        Console.WriteLine("✅ Apply completed successfully");
    }

    /// <summary>
    /// Destroy Terraform-managed infrastructure
    /// </summary>
    /// <param name="autoApprove">-auto-approve, Skip interactive approval</param>
    public async Task Destroy(bool autoApprove = false)
    {
        Console.WriteLine("💥 Destroying Terraform-managed infrastructure...");

        var args = "terraform destroy";
        if (autoApprove) args += " -auto-approve";

        await ProcessX.StartAsync(args).WriteLineAllAsync();

        Console.WriteLine("✅ Destroy completed");
    }

    /// <summary>
    /// Show current state or resource
    /// </summary>
    /// <param name="json">-json, Output in JSON format</param>
    /// <param name="path">Path to plan file or state file to show</param>
    public async Task Show(bool json = false, string? path = null)
    {
        var args = "terraform show";
        if (json) args += " -json";
        if (!string.IsNullOrEmpty(path)) args += $" {path}";

        await args;
    }

    /// <summary>
    /// List resources in state
    /// </summary>
    public async Task StateList()
    {
        Console.WriteLine("📦 Listing resources in state...");
        await "terraform state list";
    }

    /// <summary>
    /// Show outputs from state
    /// </summary>
    /// <param name="json">-json, Output in JSON format</param>
    public async Task Output(bool json = false)
    {
        var args = "terraform output";
        if (json) args += " -json";

        await args;
    }

    /// <summary>
    /// Complete workflow: init, validate, plan, and apply
    /// </summary>
    /// <param name="autoApprove">-auto-approve, Skip interactive approval for apply</param>
    public async Task Deploy(bool autoApprove = false)
    {
        Console.WriteLine("🚀 Starting complete deployment workflow...");

        await Init();
        await Validate();
        await Plan();
        await Apply(autoApprove);

        Console.WriteLine("🎉 Deployment workflow completed!");
    }
}
