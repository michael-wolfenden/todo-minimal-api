using System.Runtime.CompilerServices;

namespace TodoApi.Tests.Infrastructure;

public static class Verify
{
    public readonly static VerifySettings DefaultSettings = new VerifySettings()
        // uncomment below to auto accept all changes
        // .CloneAndSet(x => x.AutoVerify())
        // Put verification files in a Verifications subdirectory
        .CloneAndSet(x => x.UseDirectory("Verifications"))
        // Problem details middleware returns a traceId, ignoring from snapshots as it's non deterministic 
        .CloneAndSet(x => x.IgnoreMember("traceId"));

    [ModuleInitializer]
    public static void Init()
    {
        VerifyHttp.Enable();
    }

    private static VerifySettings CloneAndSet(this VerifySettings settings, Action<VerifySettings> configure)
    {
        var clonedSettings = new VerifySettings(settings);
        configure.Invoke(clonedSettings);
        return clonedSettings;
    }

    public static VerifySettings WithUseParameters(this VerifySettings settings, params object?[] parameters) =>
        settings.CloneAndSet(x => x.UseParameters(parameters));

    public static VerifySettings WithUseTextForParameters(this VerifySettings settings, string parametersText) =>
        settings.CloneAndSet(x => x.UseTextForParameters(parametersText));    
}
