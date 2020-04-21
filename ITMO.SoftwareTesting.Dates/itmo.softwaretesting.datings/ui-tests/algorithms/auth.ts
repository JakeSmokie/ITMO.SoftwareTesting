import { url } from "../utils/selenium";
import { By, until, WebDriver } from "selenium-webdriver";

export const logoutButton = (browser: WebDriver) => browser.findElement(By.id('logout-button'));
export const signInNickname = (browser: WebDriver) => browser.findElement(By.id('sign-in-nickname'));
export const signInPassword = (browser: WebDriver) => browser.findElement(By.id('sign-in-password'));
export const signInButton = (browser: WebDriver) => browser.findElement(By.id('sign-in-button'));

export const signUpNickname = (browser: WebDriver) => browser.findElement(By.id('sign-up-nickname'));
export const signUpPassword = (browser: WebDriver) => browser.findElement(By.id('sign-up-password'));
export const signUpConfirmation = (browser: WebDriver) => browser.findElement(By.id('sign-up-confirmation'));
export const signUpButton = (browser: WebDriver) => browser.findElement(By.id('sign-up-button'));

export const signUp = async (browser: WebDriver, nickname: string, password: string) => {
	await browser.get(url());

	await signUpNickname(browser).sendKeys(nickname);
	await signUpPassword(browser).sendKeys(password);
	await signUpConfirmation(browser).sendKeys(password);

	await signUpButton(browser).click();
	await browser.wait(until.urlIs(url()));
};

export const logout = async (browser: WebDriver) => {
	await logoutButton(browser).click();
	await browser.wait(until.urlIs(url('auth')));
};

export const signIn = async (browser: WebDriver, nickname: string, password: string) => {
	await browser.get(url('auth'));

	await signInNickname(browser).sendKeys(nickname);
	await signInPassword(browser).sendKeys(password);

	await signInButton(browser).click();
	await browser.wait(until.urlIs(url()));
};
