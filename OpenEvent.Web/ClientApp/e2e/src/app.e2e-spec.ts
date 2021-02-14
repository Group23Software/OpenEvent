import { AppPage } from './app.po';

describe('App', () => {
  let page: AppPage;

  beforeEach(() => {
    page = new AppPage();
  });

  it('should redirect to login page', () => {
    page.navigateTo();
    expect(page.getLoginComponent()).toBeTruthy();
  });
});
