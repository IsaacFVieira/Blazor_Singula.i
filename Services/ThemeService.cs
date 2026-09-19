using Microsoft.JSInterop;

namespace Singular.Services
{
    public class ThemeService
    {
        private readonly IJSRuntime _jsRuntime;
        private const string ThemeKey = "theme";

        public event Action? OnThemeChanged;

        public ThemeService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task<string> GetCurrentThemeAsync()
        {
            try
            {
                return await _jsRuntime.InvokeAsync<string?>(
                    "localStorage.getItem",
                    ThemeKey
                ) ?? "dark";
            }
            catch (InvalidOperationException)
            {
                // Durante o prerendering o JavaScript ainda não está disponível.
                return "dark";
            }
        }

        public async Task SetThemeAsync(string theme)
        {
            try
            {
                await _jsRuntime.InvokeVoidAsync(
                    "localStorage.setItem",
                    ThemeKey,
                    theme
                );

                await ApplyThemeAsync(theme);

                OnThemeChanged?.Invoke();
            }
            catch (InvalidOperationException)
            {
                // JavaScript ainda não está disponível durante o prerendering.
            }
        }

        public async Task ApplyThemeAsync(string theme)
        {
            try
            {
                await _jsRuntime.InvokeVoidAsync(
                    "eval",
                    $"document.documentElement.setAttribute('data-theme', '{theme}')"
                );
            }
            catch (InvalidOperationException)
            {
                // JavaScript ainda não está disponível durante o prerendering.
            }
        }

        public async Task InitializeThemeAsync()
        {
            try
            {
                var theme = await GetCurrentThemeAsync();
                await ApplyThemeAsync(theme);
            }
            catch (InvalidOperationException)
            {
                // JavaScript ainda não está disponível durante o prerendering.
            }
        }

        public async Task ToggleThemeAsync()
        {
            try
            {
                var currentTheme = await GetCurrentThemeAsync();

                var newTheme = currentTheme == "dark"
                    ? "light"
                    : "dark";

                await SetThemeAsync(newTheme);
            }
            catch (InvalidOperationException)
            {
                // JavaScript ainda não está disponível durante o prerendering.
            }
        }
    }
}