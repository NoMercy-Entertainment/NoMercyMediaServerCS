using NoMercy.Database;
using NoMercy.Database.Models;
using NoMercy.NmSystem;

namespace NoMercy.Server
{
    public static class UserSettings
    {
        public static bool TryGetUserSettings(out Dictionary<string, string> settings)
        {
            settings = new Dictionary<string, string>();

            try
            {
                using MediaContext mediaContext = new();
                List<Configuration> configuration = mediaContext.Configuration.ToList();

                foreach (Configuration? config in configuration)
                {
                    settings[config.Key] = config.Value;
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void ApplySettings(Dictionary<string, string> settings)
        {
            foreach (var setting in settings)
            {
                Logger.App($"Configuration: {setting.Key} = {setting.Value}");
                switch (setting.Key)
                {
                    case "InternalServerPort":
                        Config.InternalServerPort = int.Parse(setting.Value);
                        break;
                    case "ExternalServerPort":
                        Config.ExternalServerPort = int.Parse(setting.Value);
                        break;
                    case "queueRunners":
                        Config.QueueWorkers = new(Config.QueueWorkers.Key, setting.Value.ToInt());
                        break;
                    case "encoderRunners":
                        Config.EncoderWorkers = new(Config.EncoderWorkers.Key, setting.Value.ToInt());
                        break;
                    case "cronRunners":
                        Config.CronWorkers = new(Config.CronWorkers.Key, setting.Value.ToInt());
                        break;
                    case "dataRunners":
                        Config.DataWorkers = new(Config.DataWorkers.Key, setting.Value.ToInt());
                        break;
                    case "imageRunners":
                        Config.ImageWorkers = new(Config.ImageWorkers.Key, setting.Value.ToInt());
                        break;
                    case "requestRunners":
                        Config.RequestWorkers = new(Config.RequestWorkers.Key, setting.Value.ToInt());
                        break;
                }
            }
        }
    }
}