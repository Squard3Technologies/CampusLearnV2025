import { test, expect } from '@playwright/test';

test('test', async ({ page }) => {
  await page.goto('http://localhost:4200/home');
  await page.getByRole('link', { name: 'Login' }).click();
  await page.getByRole('textbox', { name: 'Email address' }).click();
  await page.getByRole('textbox', { name: 'Email address' }).fill('asdfa');
  await page.getByRole('textbox', { name: 'Password' }).click();
  await page.getByRole('textbox', { name: 'Password' }).fill('asdf');
  await page.getByRole('button', { name: 'Login' }).click();
  await page.getByRole('heading', { name: 'Welcome to Campus Learn' }).click();
  await page.getByRole('link', { name: 'View' }).first().click();
  await page.getByRole('heading', { name: 'Module Topics' }).click();
  await page.getByRole('row', { name: 'Introduction to Angular View' }).getByRole('button').click();
  await page.getByRole('button', { name: 'Welcome, Demo User' }).click();
  await page.getByText('Logout').click();
  await page.getByRole('heading', { name: 'Campus Learn Login' }).click();
});