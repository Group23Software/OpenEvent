import {LoginPage} from "./login.po";

describe('Login', () => {
  let page: LoginPage;

  beforeEach(() => {
    page = new LoginPage();
  });

  it('should load login page', () => {
    page.navigateToLogin();
    expect(page.getLoginComponent()).toBeTruthy();
  });

  it('should load login inputs', () =>
  {
    page.navigateToLogin();
    expect(page.getLoginComponent()).toBeTruthy();
    expect(page.getPasswordInput()).toBeTruthy();
  });

  it('should open create account dialog', () =>
  {
    page.navigateToLogin();
    page.getSignUpButton().click();
    expect(page.getCreateAccountDialog).toBeTruthy();
  });
});
