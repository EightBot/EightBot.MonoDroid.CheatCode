using System;

namespace EightBot.MonoDroid.CheatCode
{
    public static class CodeHelper
    {
        public const String KEY_UP = "UP",
                            KEY_DOWN = "DOWN",
                            KEY_LEFT = "LEFT",
                            KEY_RIGHT = "RIGHT",
                            KEY_TAP = "TAP";

        public static readonly String[] KonamiCode =
            {
                KEY_UP, KEY_UP,
                KEY_DOWN, KEY_DOWN,
                KEY_LEFT, KEY_RIGHT, KEY_LEFT, KEY_RIGHT,
                KEY_TAP, KEY_TAP, KEY_TAP
            };

        public static readonly String[] MortalKombatCode =
            {
                KEY_DOWN, KEY_UP,
                KEY_LEFT, KEY_LEFT,
                KEY_TAP,
                KEY_RIGHT, KEY_DOWN
            };
    }
}