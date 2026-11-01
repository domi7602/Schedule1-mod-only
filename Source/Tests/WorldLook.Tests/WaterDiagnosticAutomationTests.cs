using System;
using System.Text.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using Xunit;

namespace WorldLook.Tests;

[Collection("Automation")]
public sealed class WaterDiagnosticAutomationTests
{
    [Fact]
    public void ReadyWorldRoutesDiagnosticModesAndRestoresOff()
    {
        SceneManager.Active = new("Main");
        var mod = new WorldLookMod();
        mod.OnInitializeMelon();
        mod.OnLateInitializeMelon();
        try
        {
            var endpoint = GameObject.Find("WorldLook.Automation")!.GetComponent<WorldLookAutomation>();
            foreach (var (name, mode) in new[] { ("EnableWaterDepthDiagnostic", 1), ("EnableWaterWeightsDiagnostic", 2), ("DisableWaterDiagnostic", 0) })
            {
                var method = typeof(WorldLookAutomation).GetMethod(name, Type.EmptyTypes)!;
                using var response = JsonDocument.Parse((string)method.Invoke(endpoint, null)!);
                Assert.True(response.RootElement.GetProperty("accepted").GetBoolean());
                Assert.Equal(mode, WaterShader.WaterShaderMod.Latest.DiagnosticMode);
            }
        }
        finally { mod.OnDeinitializeMelon(); }
    }

    [Fact]
    public void DiagnosticFailureIsRefusedWithoutReportingAChange()
    {
        SceneManager.Active = new("Main");
        var mod = new WorldLookMod();
        mod.OnInitializeMelon();
        mod.OnLateInitializeMelon();
        try
        {
            WaterShader.WaterShaderMod.Latest.DiagnosticThrows = true;
            var endpoint = GameObject.Find("WorldLook.Automation")!.GetComponent<WorldLookAutomation>();
            using var response = JsonDocument.Parse(endpoint.EnableWaterDepthDiagnostic());
            Assert.False(response.RootElement.GetProperty("accepted").GetBoolean());
            Assert.False(response.RootElement.GetProperty("changed").GetBoolean());
            Assert.Contains("NotSupportedException", response.RootElement.GetProperty("error").GetString());
            Assert.Equal(0, WaterShader.WaterShaderMod.Latest.DiagnosticMode);
        }
        finally { mod.OnDeinitializeMelon(); }
    }

    [Fact]
    public void ParameterlessDiagnosticModesRefuseUnreadyWorld()
    {
        SceneManager.Active = new("Menu");
        var mod = new WorldLookMod();
        mod.OnInitializeMelon();
        mod.OnLateInitializeMelon();
        try
        {
            var endpoint = GameObject.Find("WorldLook.Automation")!.GetComponent<WorldLookAutomation>();
            foreach (string name in new[] { "EnableWaterDepthDiagnostic", "EnableWaterWeightsDiagnostic", "DisableWaterDiagnostic" })
            {
                var method = typeof(WorldLookAutomation).GetMethod(name, Type.EmptyTypes);
                Assert.True(method != null, $"Missing parameterless method {name}");
                using var response = JsonDocument.Parse((string)method!.Invoke(endpoint, null)!);
                Assert.False(response.RootElement.GetProperty("accepted").GetBoolean());
                Assert.Equal("not-in-main", response.RootElement.GetProperty("error").GetString());
            }
        }
        finally { mod.OnDeinitializeMelon(); SceneManager.Active = new("Main"); }
    }
}
