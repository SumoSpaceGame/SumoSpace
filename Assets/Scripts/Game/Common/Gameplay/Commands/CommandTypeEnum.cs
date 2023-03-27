using Game.Common.Gameplay.Commands.GenericCommands;
using Game.Ships.Agility.Client.CommandPerformers;
using Game.Ships.Agility.Server.CommandPerformers;
using Game.Ships.Heavy.Client.CommandPerformers;
using Game.Ships.Heavy.Server.CommandPerformers;
using UnityEditor.Rendering;

namespace Game.Common.Gameplay.Commands
{
    
    public static class CommandTypes
    {
        public  struct CommandInfo
        {
            public CommandInfo(string key, ICommandPerformer performer, ICommand receiver)
            {
                this.key = key;
                this.performer = performer;
                this.receiver = receiver;
            }

            public string key;
            public ICommandPerformer performer;
            public ICommand receiver;
        }   
        
        // Right now we are programatically adding command types to this list.
        // Eventually in the future we can add them in via a scriptable object.
        // This is just easier to keep track of for now!
        
        // The idea of using strings and registering them to a Client and Server script still stands.
        // The way these are generated and stored later may change. Maybe good ole scriptable objects?
        
        public const string COMMAND_AGILITY_DODGE = "AGILITY_DODGE";
        public const string COMMAND_AGILITY_PRIMARY_FIRE_START = "AGILITY_PRIMARY_FIRE_START";
        public const string COMMAND_AGILITY_PRIMARY_FIRE_END = "AGILITY_PRIMARY_FIRE_END";
        public const string COMMAND_AGILITY_MICRO_MISSILES = "AGILITY_MICRO_MISSILES";
        
        public const string COMMAND_HEAVY_PRIMARY_FIRE_START = "HEAVY_PRIMARY_FIRE_START";
        public const string COMMAND_HEAVY_PRIMARY_FIRE_END = "HEAVY_PRIMARY_FIRE_END";
        public const string COMMAND_HEAVY_LOCKDOWN = "HEAVY_LOCKDOWN";
        public const string COMMAND_HEAVY_BURST = "HEAVY_BURST";

        public static CommandInfo[] GetList()
        {

            var ClientGenericActivate = new ClientGenericActivateCommand();
            var ClientGenericActivateQuickPerform = new ClientGenericActivateCommand(new ClientGenericActivateCommand.Settings()
            {
                DoQuickExecuteOnPerform = true
            });
            var ServerGenericActivate = new ServerGenericActivateCommand();
            
            return new CommandInfo[] {
                
                // Agility
                new(COMMAND_AGILITY_DODGE, ClientGenericActivate, ServerGenericActivate),
                new(COMMAND_AGILITY_PRIMARY_FIRE_START, ClientGenericActivateQuickPerform, ServerGenericActivate),
                new(COMMAND_AGILITY_PRIMARY_FIRE_END, ClientGenericActivate, ServerGenericActivate),
                new(COMMAND_AGILITY_MICRO_MISSILES, ClientGenericActivate, ServerGenericActivate),

                // Heavy
                new(COMMAND_HEAVY_PRIMARY_FIRE_START, ClientGenericActivateQuickPerform, ServerGenericActivate),
                new(COMMAND_HEAVY_PRIMARY_FIRE_END, ClientGenericActivate, ServerGenericActivate),
                new(COMMAND_HEAVY_LOCKDOWN, ClientGenericActivateQuickPerform, ServerGenericActivate),
                new(COMMAND_HEAVY_BURST, ClientGenericActivate, ServerGenericActivate),
            };
        }
        
        
    }
}