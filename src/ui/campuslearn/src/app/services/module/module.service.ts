import { Injectable } from '@angular/core';
import { Observable, of, delay } from 'rxjs';
import { Module } from '../../models/module/module.model';
import { UserModule } from '../../models/module/user-module.model';

@Injectable({
  providedIn: 'root'
})
export class ModuleService {
  private mockModules: Module[] = [
    { id: '1', code: 'CS101', name: 'Introduction to Computer Science' },
    { id: '2', code: 'CS201', name: 'Data Structures and Algorithms' },
    { id: '3', code: 'CS301', name: 'Database Systems' },
    { id: '4', code: 'CS401', name: 'Software Engineering' },
    { id: '5', code: 'MATH101', name: 'Calculus I' }
  ];

  private mockUserModules: UserModule[] = [
    { userId: '1', moduleId: '1' },
    { userId: '1', moduleId: '3' },
    { userId: '2', moduleId: '1' },
    { userId: '2', moduleId: '2' }
  ];

  constructor() { }

  getAllModules(): Observable<Module[]> {
    return of(this.mockModules).pipe(delay(500));
  }

  getModuleById(id: string): Observable<Module | undefined> {
    const module = this.mockModules.find(m => m.id === id);
    return of(module).pipe(delay(300));
  }

  getModulesByUser(userId: string): Observable<Module[]> {
    const userModuleIds = this.mockUserModules
      .filter(um => um.userId === userId)
      .map(um => um.moduleId);

    const modules = this.mockModules
      .filter(module => userModuleIds.includes(module.id));

    return of(modules).pipe(delay(500));
  }

  createModule(module: Partial<Module>): Observable<Module> {
    const newModule: Module = {
      id: Date.now().toString(),
      code: module.code || '',
      name: module.name || ''
    };

    this.mockModules.push(newModule);
    return of(newModule).pipe(delay(500));
  }

  updateModule(id: string, module: Partial<Module>): Observable<Module | undefined> {
    const index = this.mockModules.findIndex(m => m.id === id);
    if (index !== -1) {
      this.mockModules[index] = {
        ...this.mockModules[index],
        ...module
      };
      return of(this.mockModules[index]).pipe(delay(500));
    }
    return of(undefined).pipe(delay(300));
  }

  deleteModule(id: string): Observable<boolean> {
    const initialLength = this.mockModules.length;
    this.mockModules = this.mockModules.filter(m => m.id !== id);
    // Also remove user-module associations
    this.mockUserModules = this.mockUserModules.filter(um => um.moduleId !== id);

    return of(initialLength > this.mockModules.length).pipe(delay(500));
  }

  assignModuleToUser(userId: string, moduleId: string): Observable<UserModule> {
    const exists = this.mockUserModules.some(
      um => um.userId === userId && um.moduleId === moduleId
    );

    if (!exists) {
      const newUserModule: UserModule = { userId, moduleId };
      this.mockUserModules.push(newUserModule);
      return of(newUserModule).pipe(delay(500));
    }

    // Return the existing user-module association
    return of({ userId, moduleId }).pipe(delay(300));
  }

  removeModuleFromUser(userId: string, moduleId: string): Observable<boolean> {
    const initialLength = this.mockUserModules.length;
    this.mockUserModules = this.mockUserModules.filter(
      um => !(um.userId === userId && um.moduleId === moduleId)
    );

    return of(initialLength > this.mockUserModules.length).pipe(delay(500));
  }
}
