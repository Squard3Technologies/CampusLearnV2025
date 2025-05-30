import { test, expect } from '@playwright/test';

test('test', async ({ page }) => {
  await page.goto('http://localhost:4200/home');
  await page.getByRole('link', { name: 'Login' }).click();
  await page.getByRole('textbox', { name: 'Email address' }).click();
  await page.getByRole('textbox', { name: 'Email address' }).fill('bailey.coding@gmail.com');
  await page.getByRole('textbox', { name: 'Email address' }).press('Tab');
  await page.getByRole('textbox', { name: 'Password' }).fill('hello');
  await page.getByRole('textbox', { name: 'Password' }).press('Enter');
  await page.getByRole('button', { name: 'Login' }).click();
  await page.getByRole('button', { name: 'Welcome, bailey brooke' }).click();
  await page.getByRole('link', { name: 'Profile' }).click();
  await page.getByRole('heading', { name: 'My Profile' }).click();
  await page.getByText('Tutor Subscription').click();
  await page.getByRole('heading', { name: 'My Tutor Subscriptions' }).click();
  await page.getByText('Topic Subscription').click();
  await page.getByRole('heading', { name: 'My Subscribed Topics' }).click();
  await page.getByRole('main').getByText('Logout').click();
  await page.getByRole('heading', { name: 'Campus Learn Login' }).click();
});