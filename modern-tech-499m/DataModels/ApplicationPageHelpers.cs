using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using modern_tech_499m.Pages;
using modern_tech_499m.ViewModels;

namespace modern_tech_499m
{
    public static class ApplicationPageHelpers
    {
        public static BasePage ToBasePage(this ApplicationPage page, object viewmodel = null)
        {
            switch (page)
            {

                case ApplicationPage.Login:
                    return new LoginPage(viewmodel as LoginViewModel);

                case ApplicationPage.Register:
                    return new RegisterPage(viewmodel as RegisterViewModel);

                case ApplicationPage.Game:
                    return new GamePage(viewmodel as GamePageViewModel);

                case ApplicationPage.GameInfoSelection:
                    return new GameInfoSelectionPage();

                case ApplicationPage.PlayerSelection:
                    return null;

                case ApplicationPage.UsersDatabase:
                    return null;

                case ApplicationPage.Welcome:
                    return new WelcomePage(viewmodel as WelcomePageViewModel);

                default:
                    Debugger.Break();
                    return null;
            }
        }

        public static ApplicationPage ToApplicationPage(this BasePage page)
        {
            switch (page)
            {
                case LoginPage _: return ApplicationPage.Login;
                case RegisterPage _: return ApplicationPage.Register;
                case GamePage _: return ApplicationPage.Game;
                case GameInfoSelectionPage _: return ApplicationPage.GameInfoSelection;
                case WelcomePage _: return ApplicationPage.Welcome;
            }
            Debugger.Break();
            return default;
        }
    }
}
