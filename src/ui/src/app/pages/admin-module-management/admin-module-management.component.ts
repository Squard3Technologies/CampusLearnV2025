import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { ApiService } from '../../services/api.service';
import Swal from 'sweetalert2';
import { Module } from '../../models/api.models';


@Component({
  selector: 'app-admin-module-management',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './admin-module-management.component.html',
  styleUrl: './admin-module-management.component.scss'
})
export class AdminModuleManagementComponent implements OnInit {
  showModal = false;
  showEditModal = false;
  moduleId = '';
  moduleCode = '';
  moduleName = '';
  errorMessage = '';


  roleFilter: number = 0;
  statusFilter: boolean | null = null;
  searchTerm: string = '';
  currentPage: number = 1;
  pageSize: number = 5;
  totalPages: number = 1;

  modules: Module[] = [];
  filteredModules: Module[] = [];
  pagedModules: any[] = [];


  constructor(
    private router: Router,
    private apiService: ApiService
  ) { }


  ngOnInit(): void {
    this.filterModules();
    this.loadModules();
  }

  loadModules(): void {
    // No data for now - empty table
    this.apiService.getAdminModules().subscribe({
      next: (response) => {
        if (response != null) {
          if (!response.status) {
            Swal.fire({
              icon: 'error',
              iconColor: '#dc3545',
              title: 'GETTING MODULES ERROR',
              text: response.statusMessage,
              confirmButtonColor: '#dc3545',
              confirmButtonText: '<i class="fa fa-thumbs-down me-2"></i> Dismiss',
              allowOutsideClick: false,
              buttonsStyling: false,
              customClass: {
                confirmButton: 'btn btn-outline-danger me-2',
              }
            });
            return;
          }

          if (response.body == null) {
            Swal.fire({
              icon: 'error',
              iconColor: '#AD0151',
              title: 'GETTING MODULES ERROR',
              text: response?.statusMessage || 'Retrieving modules failed. Please try again.',
              background: "#dc3545",
              color: "#fafafa",
              confirmButtonColor: '#AD0151',
              confirmButtonText: 'Dismiss',
              allowOutsideClick: false
            });
            return;
          }
          this.modules = response.body;
          this.filteredModules = response.body;
          this.filterModules();
        }
        else {
          Swal.fire({
            icon: 'error',
            iconColor: '#dc3545',
            title: 'ERROR',
            html: '<p class="font-13">Internal system error encountered loading modules</p>',
            confirmButtonColor: '#dc3545',
            confirmButtonText: '<i class="fa fa-thumbs-down me-2"></i> Dismiss',
            allowOutsideClick: false,
            buttonsStyling: false,
            customClass: {
              confirmButton: 'btn btn-md btn-outline-danger me-2',
            }
          }).then((result) => {

          });
        }
      },
      error: (error) => {

      }
    });
  }

  navigateToUserManagement(): void {
    this.router.navigate(['/admin/users']);
  }

  navigateToPendingRegistrations(): void {
    this.router.navigate(['/admin/registrations']);
  }



  /** Opens modal for new enquiry */
  openCreateModuleModal(): void {
    this.showModal = true;
  }

  openEditModuleModal(module: Module): void {
    this.moduleId = module.id;
    this.moduleCode = module.code;
    this.moduleName = module.name;
    this.showEditModal = true;
  }


  /** Closes modal */
  closeCreateModuleModal(reset: boolean, editModule: boolean) {
    if (reset) {
      this.moduleCode = '';
      this.moduleName = '';
    }
    if (editModule) {
      this.moduleId = '';
      this.showEditModal = false;
    }
    else {
      this.showModal = false;
    }
  }


  /** Invoke API to create a module */
  submitCreateModule() {
    if (!this.moduleCode || !this.moduleName) {
      this.errorMessage = 'Please fill in all fields';
      Swal.fire({
        icon: 'error',
        iconColor: '#AD0151',
        title: 'Validation Error',
        text: 'Please fill in all fields',
        confirmButtonColor: '#dc3545',
        confirmButtonText: '<i class="fa fa-thumbs-down me-2"></i> Dismiss',
        allowOutsideClick: false,
        buttonsStyling: false,
        customClass: {
          confirmButton: 'btn btn-md btn-outline-danger me-2',
        }
      });
      return;
    }

    const module = {
      code: this.moduleCode,
      name: this.moduleName
    }

    this.closeCreateModuleModal(false, false);
    console.log('new module data:', module);
    Swal.fire({
      title: 'Creating module...',
      allowOutsideClick: false,
      didOpen: () => {
        Swal.showLoading();
      }
    });

    // Call API registration service
    this.apiService.createModule(module).subscribe({
      next: (response) => {
        console.log('module creation successful:', response);
        Swal.close();
        if (response != null) {
          if (!response.status) {
            this.openCreateModuleModal();
            Swal.fire({
              icon: 'error',
              iconColor: '#dc3545',
              title: 'CREATE MODULE ERROR',
              text: response.statusMessage,
              confirmButtonColor: '#dc3545',
              confirmButtonText: '<i class="fa fa-thumbs-down me-2"></i> Dismiss',
              allowOutsideClick: false,
              buttonsStyling: false,
              customClass: {
                confirmButton: 'btn btn-outline-danger me-2',
              }
            });
            return;
          }
          Swal.fire({
            icon: 'success',
            iconColor: '#198754',
            title: 'SUCCESS',
            html: response.statusMessage,
            confirmButtonColor: '#fafafa',
            confirmButtonText: '<i class="fa fa-thumbs-up me-2"></i> Dismiss',
            allowOutsideClick: false,
            buttonsStyling: false,
            customClass: {
              confirmButton: 'btn btn-md btn-outline-success me-2',
            }
          }).then((result) => {
            this.moduleCode = '';
            this.moduleName = '';
            this.loadModules();
          });

        }
        else {
          this.openCreateModuleModal();
          Swal.fire({
            icon: 'error',
            iconColor: '#dc3545',
            title: 'ERROR',
            html: '<p class="font-13">Create Module failed. <br/>Interna system error encountered</p>',
            confirmButtonColor: '#dc3545',
            confirmButtonText: '<i class="fa fa-thumbs-down me-2"></i> Dismiss',
            allowOutsideClick: false,
            buttonsStyling: false,
            customClass: {
              confirmButton: 'btn btn-md btn-outline-danger me-2',
            }
          }).then((result) => {

          });
        }

      },
      error: (error) => {
        this.openCreateModuleModal();
        this.errorMessage = error.error?.message || 'Module creation failed. Please try again.';
        Swal.close();
        Swal.fire({
          icon: 'error',
          iconColor: '#dc3545',
          title: 'CREATE MODULE ERROR',
          text: this.errorMessage,
          confirmButtonColor: '#dc3545',
          confirmButtonText: '<i class="fa fa-thumbs-down me-2"></i> Dismiss',
          allowOutsideClick: false,
          buttonsStyling: false,
          customClass: {
            confirmButton: 'btn btn-md btn-outline-danger me-2',
          }
        });
      }

    });


  }


  submitEditModule() {
    if (!this.moduleId || !this.moduleCode || !this.moduleName) {
      this.errorMessage = 'Please fill in all fields';
      Swal.fire({
        icon: 'error',
        iconColor: '#AD0151',
        title: 'Validation Error',
        text: 'Please fill in all fields',
        confirmButtonColor: '#dc3545',
        confirmButtonText: '<i class="fa fa-thumbs-down me-2"></i> Dismiss',
        allowOutsideClick: false,
        buttonsStyling: false,
        customClass: {
          confirmButton: 'btn btn-md btn-outline-danger me-2',
        }
      });
      return;
    }

    const module = {
      id: this.moduleId,
      code: this.moduleCode,
      name: this.moduleName
    }

    this.closeCreateModuleModal(false, true);
    console.log('edit module data:', module);
    Swal.fire({
      title: 'Saving module changes...',
      allowOutsideClick: false,
      didOpen: () => {
        Swal.showLoading();
      }
    });

    // Call API registration service
    this.apiService.updateModule(module.id, module).subscribe({
      next: (response) => {
        console.log('module update successful:', response);
        Swal.close();
        if (response != null) {
          if (!response.status) {
            this.openCreateModuleModal();
            Swal.fire({
              icon: 'error',
              iconColor: '#dc3545',
              title: 'UPDATE MODULE ERROR',
              text: response.statusMessage,
              confirmButtonColor: '#dc3545',
              confirmButtonText: '<i class="fa fa-thumbs-down me-2"></i> Dismiss',
              allowOutsideClick: false,
              buttonsStyling: false,
              customClass: {
                confirmButton: 'btn btn-outline-danger me-2',
              }
            });
            return;
          }
          Swal.fire({
            icon: 'success',
            iconColor: '#198754',
            title: 'SUCCESS',
            html: response.statusMessage,
            confirmButtonColor: '#fafafa',
            confirmButtonText: '<i class="fa fa-thumbs-up me-2"></i> Dismiss',
            allowOutsideClick: false,
            buttonsStyling: false,
            customClass: {
              confirmButton: 'btn btn-md btn-outline-success me-2',
            }
          }).then((result) => {
            this.moduleCode = '';
            this.moduleName = '';
            this.loadModules();
          });

        }
        else {
          this.openCreateModuleModal();
          Swal.fire({
            icon: 'error',
            iconColor: '#dc3545',
            title: 'ERROR',
            html: '<p class="font-13">Updating Module failed. <br/>Internal system error encountered</p>',
            confirmButtonColor: '#dc3545',
            confirmButtonText: '<i class="fa fa-thumbs-down me-2"></i> Dismiss',
            allowOutsideClick: false,
            buttonsStyling: false,
            customClass: {
              confirmButton: 'btn btn-md btn-outline-danger me-2',
            }
          }).then((result) => {

          });
        }

      },
      error: (error) => {
        this.openCreateModuleModal();
        this.errorMessage = error.error?.message || 'Module update failed. Please try again.';
        Swal.close();
        Swal.fire({
          icon: 'error',
          iconColor: '#dc3545',
          title: 'UPDATE MODULE ERROR',
          text: this.errorMessage,
          confirmButtonColor: '#dc3545',
          confirmButtonText: '<i class="fa fa-thumbs-down me-2"></i> Dismiss',
          allowOutsideClick: false,
          buttonsStyling: false,
          customClass: {
            confirmButton: 'btn btn-md btn-outline-danger me-2',
          }
        });
      }

    });

  }


  toggleModuleStatus(module: Module, status: boolean) {
    debugger;
    if (status) {
      this.apiService.activateModule(module.id).subscribe({
        next: (response) => {
          this.loadModules();
        }
      });
    }
    else {
      this.apiService.deactivateModule(module.id).subscribe({
        next: (response) => {
          this.loadModules();
        }
      });
    }

  }



  filterModules(): void {
    let filtered = this.modules;

    console.log(`selected role: ${this.roleFilter}`);
    console.log(`select status: ${this.statusFilter}`);

    // Apply search filter
    if (this.searchTerm) {
      const term = this.searchTerm.toLowerCase();
      filtered = filtered.filter(module =>
        module.code.toLowerCase().includes(term) ||
        module.name.toLowerCase().includes(term)
      );
    }

    debugger;
    // Apply status filter
    debugger;
    if (this.statusFilter) {
      console.log(`selected filer: ${this.statusFilter}`);
      filtered = filtered.filter(module => module.status.toString() === this.statusFilter?.toString());
    }

    this.filteredModules = filtered;
    this.updatePagination();
  }


  updatePagination(): void {
    this.totalPages = Math.ceil(this.filteredModules.length / this.pageSize);
    if (this.currentPage > this.totalPages) {
      this.currentPage = 1;
    }

    this.totalPages = Math.ceil(this.filteredModules.length / this.pageSize);

    // Bound currentPage to valid range
    if (this.currentPage > this.totalPages) this.currentPage = this.totalPages || 1;
    if (this.currentPage < 1) this.currentPage = 1;

    const startIndex = (this.currentPage - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    this.pagedModules = this.filteredModules.slice(startIndex, endIndex);
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.updatePagination();
    }
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.updatePagination();
    }
  }

  Math = Math;

}
