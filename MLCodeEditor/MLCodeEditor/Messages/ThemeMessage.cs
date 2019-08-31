using Prism.Events;

namespace MLCodeEditor.Messages
{
    class ThemeMessage : PubSubEvent<string>
    {
    }

    class fontColorThemeMessage : PubSubEvent<string>
    {
    }
     class syntaxThemeMessage : PubSubEvent<string>
    {
    }
}
