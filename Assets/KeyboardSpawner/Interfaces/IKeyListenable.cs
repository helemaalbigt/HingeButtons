using System;

public interface IKeyListenable
{
    event Action<string> OnPressed;
}
