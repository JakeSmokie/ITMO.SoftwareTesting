import { cleanLocalStorage, createWebDriver, url } from '../utils/selenium';
import { until, WebDriver } from 'selenium-webdriver';
import { assertToastText, ts } from '../utils/utils';
import {
	accountDeletionButton,
	accountDeletionPassword,
	logout,
	signIn,
	signInButton,
	signInPassword,
	signUp,
	signUpButton,
	signUpConfirmation,
	signUpNickname,
	signUpPassword,
	trySignIn,
	userPageButton,
} from '../pages/auth';

describe('auth page', () => {
	let browser: WebDriver;

	beforeAll(async () => {
		browser = await createWebDriver();
	});

	afterEach(async () => {
		await cleanLocalStorage(browser);
	});

	afterAll(async () => {
		await browser.quit();
	});

	it('should redirect from / to /auth', async () => {
		await browser.get(url());
		await browser.wait(until.urlIs(url('auth')), 200);
	});

	it('should successfully sign up', async () => {
		const nickname = ts('RandomDude');
		await signUp(browser, nickname, nickname + '_123');
	});

	it('should redirect from /auth to / page after signing up', async () => {
		const nickname = ts('RandomDude');
		await signUp(browser, nickname, nickname + '_123');

		await browser.get(url('auth'));
		await browser.wait(until.urlIs(url('')));
	});

	it('should not allow to sign up when nickname is invalid', async () => {
		const password = 'nice_password';
		await assertSignUpIsNotAvailable(browser, '', password, password);
	});

	it('should not allow to sign up when password is too short', async () => {
		const password = '123';
		await assertSignUpIsNotAvailable(browser, ts('Nice Nickname'), password, password);
	});

	it('should not allow to sign up when passwords are not the same', async () => {
		await assertSignUpIsNotAvailable(browser, ts('Nice Nickname'), 'Some nice password', 'Really weird password');
	});

	it('should successfully login', async () => {
		const nickname = ts('Nickname');
		const password = 'password_123';

		await signUp(browser, nickname, password);
		await logout(browser);
		await signIn(browser, nickname, password);
	});

	it('should redirect from /auth to / page after signing in', async () => {
		const nickname = ts('Nickname');
		const password = 'password_123';

		await signUp(browser, nickname, password);
		await logout(browser);
		await signIn(browser, nickname, password);

		await browser.get(url('auth'));
		await browser.wait(until.urlIs(url('')));
	});

	it('should throw error on wrong password', async () => {
		const nickname = ts('Nickname');

		await signUp(browser, nickname, 'password_123');
		await logout(browser);
		await trySignIn(browser, nickname, 'password_123_123');

		await assertToastText(browser, 'Incorrect password');
	});

	it('should not allow sign in on empty nickname', async () => {
		await assertSignInIsNotAvailable(browser, '', '1234567890');
	});

	it('should not allow sign in on short password', async () => {
		await assertSignInIsNotAvailable(browser, 'Some cool dude', '123');
	});


	it('should provide account deletion', async () => {
		const nickname = ts('Nickname');
		const password = 'password_123';

		await signUp(browser, nickname, password);
		await userPageButton(browser).click();

		await browser.wait(until.urlIs(url('user')));
		await accountDeletionPassword(browser).sendKeys(password);

		await accountDeletionButton(browser).click();
		await browser.wait(until.urlIs(url('auth')));

		await trySignIn(browser, nickname, password);
		await assertToastText(browser, 'No such user was found');
	});

	it('should not allow to delete an account on wrong password', async () => {
		await signUp(browser, ts('Nickname'), 'password_123');
		await userPageButton(browser).click();

		await browser.wait(until.urlIs(url('user')));
		await accountDeletionPassword(browser).sendKeys('123');
		await accountDeletionButton(browser).click();

		await assertToastText(browser, 'Incorrect password');
	});
});

const assertSignUpIsNotAvailable = async (browser: WebDriver, nickname: string, password: string, confirmation: string) => {
	await browser.get(url());

	await signUpNickname(browser).sendKeys(nickname);
	await signUpPassword(browser).sendKeys(password);
	await signUpConfirmation(browser).sendKeys(confirmation);

	expect(await signUpButton(browser).isEnabled()).toBe(false);
};

const assertSignInIsNotAvailable = async (browser: WebDriver, nickname: string, password: string) => {
	await browser.get(url());

	await signInPassword(browser).sendKeys(nickname);
	await signInPassword(browser).sendKeys(password);

	expect(await signInButton(browser).isEnabled()).toBe(false);
};
