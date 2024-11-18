using Reactor.Utilities;

namespace VanillaEnhancements;

public static class VELogger
{
    public static void Debug(string message)
    {
        Logger<VanillaEnhancementsPlugin>.Debug(message);
    }

    public static void Info(string message)
    {
        Logger<VanillaEnhancementsPlugin>.Info(message);
    }

    public static void Warn(string message)
    {
        Logger<VanillaEnhancementsPlugin>.Warning(message);
    }

    public static void Error(string message)
    {
        Logger<VanillaEnhancementsPlugin>.Error(message);
    }

    public static void Fatal(string message)
    {
        Logger<VanillaEnhancementsPlugin>.Fatal(message);
    }
}