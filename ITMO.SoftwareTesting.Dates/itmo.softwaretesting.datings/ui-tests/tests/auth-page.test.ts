import { createWebDriver, url } from '../utils/selenium';
import { until, WebDriver } from 'selenium-webdriver';
import { ts } from '../utils/utils';
import {
	logout,
	signIn,
	signUp,
	signUpButton,
	signUpConfirmation,
	signUpNickname,
	signUpPassword,
} from '../algorithms/auth';

describe('auth page', () => {
	let browser: WebDriver;

	beforeAll(async () => {
		browser = await createWebDriver();
	});

	afterEach(async () => {
		await browser.executeScript(() => {
			localStorage.clear();
		});
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

	it('should redirect to home page after signing up', async () => {
		const nickname = ts('RandomDude');
		await signUp(browser, nickname, nickname + '_123');

		await browser.get(url('auth'));
		await browser.wait(until.urlIs(url('')));
	});

	it('should not allow to sign up when nickname is invalid', async () => {
		const password = 'nice_password';
		await assertSignUpFailed(browser, '', password, password);
	});

	it('should not allow to sign up when password is too short', async () => {
		const password = '123';
		await assertSignUpFailed(browser, ts('Nice Nickname'), password, password);
	});

	it('should not allow to sign up when passwords are not the same', async () => {
		await assertSignUpFailed(browser, ts('Nice Nickname'), 'Some nice password', 'Really weird password');
	});

	it('should successfully login', async () => {
		const nickname = ts('Nickname');
		const password = 'password_123';

		await signUp(browser, nickname, password);
		await logout(browser);
		await signIn(browser, nickname, password);
	});

	it('should redirect back to home page after signing in', async () => {
		const nickname = ts('Nickname');
		const password = 'password_123';

		await signUp(browser, nickname, password);
		await logout(browser);
		await signIn(browser, nickname, password);

		await browser.get(url('auth'));
		await browser.wait(until.urlIs(url('')));
	});
});

const assertSignUpFailed = async (browser: WebDriver, nickname: string, password: string, confirmation: string) => {
	await browser.get(url());

	await signUpNickname(browser).sendKeys(nickname);
	await signUpPassword(browser).sendKeys(password);
	await signUpConfirmation(browser).sendKeys(confirmation);

	expect(await signUpButton(browser).isEnabled()).toBe(false);
};
