using OpenQA.Selenium;
using System;

namespace TestLinkTestRailMigrator.Main.Utils
{
    public class ExpectedConditions
    {
        /// <summary>
        /// An expectation for checking an element is hidden
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>True if it is hidden, false otherwise.</returns>
        public static Func<IWebDriver, bool> ElementIsHidden(IWebElement element)
        {
            return (driver) =>
            {
                return !element.Displayed;
            };
        }

        /// <summary>
        /// Function to allow javascript to run suggest/autocomplete features
        /// </summary>
        public static Func<IWebDriver, bool> ElementIsSuggested(IWebElement element, string name)
        {
            return (driver) =>
            {
                try
                {
                    return element.Text.Contains(name);
                }
                catch (StaleElementReferenceException)
                {
                    return false;
                }
            };
        }

        public static Func<IWebDriver, bool> JQueryStopped()
        {
            return (driver) =>
            {
                var j2e = (IJavaScriptExecutor)driver;
                return (bool)j2e.ExecuteScript("return jQuery.active == 0");
            };
        }

        /// <summary>
        /// An expectation for checking an element is visible 
        /// </summary>
        /// <param name="locator">The element identifier.</param>
        /// <returns>The <see cref="IWebElement"/> once it is visible.</returns>
        public static Func<IWebDriver, IWebElement> ElementIsVisible(IWebElement element)
        {
            return (webDriver) =>
            {
                try
                {
                    return ElementIfVisible(element);
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
                catch (NoSuchElementException)
                {
                    return null;
                }
                catch
                {
                    return null;
                }
            };
        }
        public static Func<IWebDriver, IWebElement> ElementToBeClickable(IWebElement element)
        {
            return (webDriver) =>
            {
                var _element = ElementIfVisible(element);
                try
                {
                    if (_element != null && _element.Enabled)
                    {
                        return _element;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
            };
        }

        public static Func<IWebDriver, IWebElement> ElementToBeClickable(By element)
        {
            return (webDriver) =>
            {
                //var _element = ElementIfVisible(webDriver.FindElement(element));
                try
                {
                    var _element = ElementIfVisible(webDriver.FindElement(element));
                    if (_element != null && _element.Enabled)
                    {
                        return _element;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch
                {
                    return null;
                }
            };
        }

        /// <summary>
        /// An expectation for checking that an element is either invisible or not present on the DOM.
        /// </summary>
        /// <param name="locator">The locator used to find the element.</param>
        /// <returns><see langword="true"/> if the element is not displayed; otherwise, <see langword="false"/>.</returns>

        public static Func<IWebDriver, bool> InvisibilityOfElementLocated(IWebElement element)
        {
            return (driver) =>
            {
                try
                {
                    return !element.Displayed;
                }
                catch (NoSuchElementException)
                {
                    // Returns true because the element is not present in DOM. The
                    // try block checks if the element is present but is invisible.
                    return true;
                }
                catch (StaleElementReferenceException)
                {
                    // Returns true because stale element reference implies that element
                    // is no longer visible.
                    return true;
                }
            };
        }

        private static IWebElement ElementIfVisible(IWebElement webElement)
        {
            return webElement.Displayed ? webElement : null;
        }
    }
}
