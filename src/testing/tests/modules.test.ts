import { test, expect } from '@playwright/test';

test('test', async ({ page }) => {
  await page.goto('http://localhost:4200/home');
  await page.getByRole('link', { name: 'Login' }).click();
  await page.getByRole('textbox', { name: 'Email address' }).click();
  await page.getByRole('textbox', { name: 'Email address' }).fill('bailey.coding@gmail.com');
  await page.getByRole('textbox', { name: 'Email address' }).press('Tab');
  await page.getByRole('textbox', { name: 'Password' }).fill('hello');
  await page.getByRole('button', { name: 'Login' }).click();
  await page.getByRole('link', { name: 'Modules' }).click();
  await page.getByRole('button', { name: 'View Module' }).first().click();
  await page.getByRole('button', { name: 'View Topic' }).first().click();
  await page.getByText('Materials').click();
  await page.getByText('Discussion').click();
  await page.getByText('Quizzes').click();
});