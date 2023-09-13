import React, { Component } from 'react';
import { variables } from './Variables';

export class Department extends Component {
  constructor(props) {
    super(props);

    this.state = {
      departments: [],
      modalTitle: "",         // Modal title for adding/editing departments
      DepartmentName: "",     // Input field for department name
      DepartmentId: 0,        // Department ID (0 for adding, actual ID for editing)

      DepartmentIdFilter:"",
      DepartmentNameFilter:"",
      departmentsWithoutFilter:[],
    };
  }
  FilterFn(){
    var DepartmentIdFilter=this.state.DepartmentIdFilter;
    var DepartmentNameFilter=this.state.DepartmentNameFilter;
    var filteredData=this.state.departmentsWithoutFilter.filter(
      function(el){
        return el.DepartmentId.toString().toLowerCase().includes(
          DepartmentIdFilter.toString().trim().toLowerCase()
        )&& 
        el.DepartmentName.toString().toLowerCase().includes(
          DepartmentNameFilter.toString().trim().toLowerCase()
        )
        
      }
    );

    this.setState({departments:filteredData});
  }

  sortResult(prop,asc){
    var sortedData=this.state.departmentsWithoutFilter.sort(function(a,b){
      if(asc){
        return (a[prop]>b[prop])?1:((a[prop]<b[prop])?-1:0);
      }
      else {
        return (b[prop]>a[prop])?1:((b[prop]<a[prop])?-1:0);
      }
    });

    this.setState({departments:sortedData});
  }
    changeDepartmentIdFilter =(e)=>{
      this.state.DepartmentIdFilter=e.target.value;
      this.FilterFn();
    }

    changeDepartmentNameFilter =(e)=>{
      this.state.DepartmentNameFilter=e.target.value;
      this.FilterFn();
    }

  // Function to refresh the list of departments by making an API request
  refreshList() {
    fetch(variables.API_URL + 'Department')
      .then((response) => response.json())
      .then((data) => {
        // Log the API response
        // Check if the API request was successful (status 200)
        if (data.status === 200) {
          this.setState({ departments: data.data, departmentsWithoutFilter:data.data }); // Update the departments in state
        } else {
          console.error('API request failed:', data.message); // Handle error if needed
        }
      })
      .catch((error) => {
        console.error('Error fetching data:', error);
      });
  }

  // Lifecycle method: Fetch the list of departments when the component mounts
  componentDidMount() {
    this.refreshList();
  }

  // Function to handle changes in the department name input field
  changeDepartmentName = (e) => {
    this.setState({ DepartmentName: e.target.value });
  };

  // Function to handle the "Edit" button click, populating the modal with department data
  editClick = (dep) => {
    this.setState({
      modalTitle: "Edit Department",        // Set modal title
      DepartmentId: dep.DepartmentId,       // Set DepartmentId in state for editing
      DepartmentName: dep.DepartmentName,  // Set DepartmentName in state for editing
    });
  };

  // Function to handle the "Add Department" button click, resetting modal fields
  addClick = () => {
    this.setState({
      modalTitle: "Add Department", // Set modal title for adding
      DepartmentId: 0,             // Reset DepartmentId (indicating a new department)
      DepartmentName: "",          // Reset DepartmentName input field
    });
  };

  // Function to handle the "Create" button click, adding a new department
  createClick() {
    fetch(variables.API_URL + 'Department', {
      method: 'POST',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        DepartmentName: this.state.DepartmentName,
      }),
    })
      .then((res) => res.json())
      .then((result) => {
        alert('Successfully added!', result); // Display success message
        this.refreshList(); // Refresh the list of departments
      })
      .catch((error) => {
        alert('Failed'); // Handle errors if any
      });
  }

  // Function to handle the "Update" button click, updating an existing department
  updateClick() {
    fetch(variables.API_URL + 'Department', {
      method: 'PUT',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        DepartmentId: this.state.DepartmentId,
        DepartmentName: this.state.DepartmentName,
      }),
    })
      .then((res) => res.json())
      .then((result) => {
        alert('Successfully updated!', result); // Display success message
        this.refreshList(); // Refresh the list of departments
      })
      .catch((error) => {
        alert('Failed'); // Handle errors if any
      });
  }

  // Function to handle the "Delete" button click, deleting a department
  deleteClick(id) {
    if (window.confirm('Are you sure?')) {
      fetch(variables.API_URL + 'Department/' + id, {
        method: 'DELETE',
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json',
        },
      })
        .then((res) => res.json())
        .then((result) => {
          alert('Successfully deleted!', result); // Display success message
          this.refreshList(); // Refresh the list of departments
        })
        .catch((error) => {
          alert('Failed'); // Handle errors if any
        });
    }
  }

  // Render the component with the department list and modal
  render() {
    const { departments, modalTitle, DepartmentName, DepartmentId } = this.state;

    return (
      <div>
        {/* Button to open the modal for adding/editing departments */}
        <button
          type='button'
          className='btn btn-primary m-2 float-end'
          data-bs-toggle='modal'
          data-bs-target='#exampleModal'
          onClick={() => this.addClick()}
        >
          Add Department
        </button>

        {/* Table to display the list of departments */}
        <table className='table table-striped'>
          <thead>
            <tr>
              <th> 
              <div className='d-flex flex-row'>
              <input className='form-control m-2' onChange={this.changeDepartmentIdFilter} 
              placeholder='Filter'/>
              
              <button type='button' className='btn btn-light' onClick={()=>this.sortResult('DepartmentId',true)}>
              <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" className="bi bi-arrow-down-square-fill" viewBox="0 0 16 16">
              <path d="M2 0a2 2 0 0 0-2 2v12a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V2a2 2 0 0 0-2-2H2zm6.5 4.5v5.793l2.146-2.147a.5.5 0 0 1 .708.708l-3 3a.5.5 0 0 1-.708 0l-3-3a.5.5 0 1 1 .708-.708L7.5 10.293V4.5a.5.5 0 0 1 1 0z"/>
            </svg>
              </button>

              <button type='button' className='btn btn-light' onClick={()=>this.sortResult('DepartmentId',false)}>
              <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" className="bi bi-arrow-up-square-fill" viewBox="0 0 16 16">
              <path d="M2 16a2 2 0 0 1-2-2V2a2 2 0 0 1 2-2h12a2 2 0 0 1 2 2v12a2 2 0 0 1-2 2H2zm6.5-4.5V5.707l2.146 2.147a.5.5 0 0 0 .708-.708l-3-3a.5.5 0 0 0-.708 0l-3 3a.5.5 0 1 0 .708.708L7.5 5.707V11.5a.5.5 0 0 0 1 0z"/>
            </svg>
              </button>
              </div>
                DepartmentId </th>
              <th>
                <div className='d-flex flex-row'>
              <input className='form-control m-2' onChange={this.changeDepartmentNameFilter} 
              placeholder='Filter'/>

              <button type='button' className='btn btn-light' onClick={()=>this.sortResult('DepartmentName',true)}>
              <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" className="bi bi-arrow-down-square-fill" viewBox="0 0 16 16">
              <path d="M2 0a2 2 0 0 0-2 2v12a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V2a2 2 0 0 0-2-2H2zm6.5 4.5v5.793l2.146-2.147a.5.5 0 0 1 .708.708l-3 3a.5.5 0 0 1-.708 0l-3-3a.5.5 0 1 1 .708-.708L7.5 10.293V4.5a.5.5 0 0 1 1 0z"/>
            </svg>
              </button>

              <button type='button' className='btn btn-light' onClick={()=>this.sortResult('DepartmentName',false)}>
              <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" className="bi bi-arrow-up-square-fill" viewBox="0 0 16 16">
              <path d="M2 16a2 2 0 0 1-2-2V2a2 2 0 0 1 2-2h12a2 2 0 0 1 2 2v12a2 2 0 0 1-2 2H2zm6.5-4.5V5.707l2.146 2.147a.5.5 0 0 0 .708-.708l-3-3a.5.5 0 0 0-.708 0l-3 3a.5.5 0 1 0 .708.708L7.5 5.707V11.5a.5.5 0 0 0 1 0z"/>
            </svg>
              </button>  
              </div> 
                 DepartmentName 
                 </th>
              <th> Options </th>
            </tr>
          </thead>
          <tbody>
            {Array.isArray(departments) && departments.length > 0 ? (
              departments.map((dep) => (
                <tr key={dep.DepartmentId}>
                  <td>{dep.DepartmentId}</td>
                  <td>{dep.DepartmentName}</td>
                  <td>
                    {/* Edit button */}
                    <button
                      type="button"
                      className='btn btn-light mr-1'
                      data-bs-toggle='modal'
                      data-bs-target='#exampleModal'
                      onClick={() => this.editClick(dep)}
                    >
                      <svg
                        xmlns="http://www.w3.org/2000/svg"
                        width="16"
                        height="16"
                        fill="currentColor"
                        className="bi bi-pencil-square"
                        viewBox="0 0 16 16"
                      >
                        {/* Edit icon */}
                        <path d="M15.502 1.94a.5.5 0 0 1 0 .706L14.459 3.69l-2-2L13.502.646a.5.5 0 0 1 .707 0l1.293 1.293zm-1.75 2.456-2-2L4.939 9.21a.5.5 0 0 0-.121.196l-.805 2.414a.25.25 0 0 0 .316.316l2.414-.805a.5.5 0 0 0 .196-.12l6.813-6.814z"/>
                        <path fillRule="evenodd" d="M1 13.5A1.5 1.5 0 0 0 2.5 15h11a1.5 1.5 0 0 0 1.5-1.5v-6a.5.5 0 0 0-1 0v6a.5.5 0 0 1-.5.5h-11a.5.5 0 0 1-.5-.5v-11a.5.5 0 0 1 .5-.5H9a.5.5 0 0 0 0-1H2.5A1.5 1.5 0 0 0 1 2.5v11z"/>
                      </svg>
                    </button>

                    {/* Delete button */}
                    <button
                      type="button"
                      className='btn btn-light mr-1'
                      data-bs-toggle='modal'
                      data-bs-target='#exampleModal'
                      onClick={() => this.deleteClick(dep.DepartmentId)}
                    >
                      <svg
                        xmlns="http://www.w3.org/2000/svg"
                        width="16"
                        height="16"
                        fill="currentColor"
                        className="bi bi-trash3-fill"
                        viewBox="0 0 16 16"
                      >
                        {/* Delete icon */}
                        <path d="M11 1.5v1h3.5a.5.5 0 0 1 0 1h-.538l-.853 10.66A2 2 0 0 1 11.115 16h-6.23a2 2 0 0 1-1.994-1.84L2.038 3.5H1.5a.5.5 0 0 1 0-1H5v-1A1.5 1.5 0 0 1 6.5 0h3A1.5 1.5 0 0 1 11 1.5Zm-5 0v1h4v-1a.5.5 0 0 0-.5-.5h-3a.5.5 0 0 0-.5.5ZM4.5 5.029l.5 8.5a.5.5 0 1 0 .998-.06l-.5-8.5a.5.5 0 1 0-.998.06Zm6.53-.528a.5.5 0 0 0-.528.47l-.5 8.5a.5.5 0 0 0 .998.058l.5-8.5a.5.5 0 0 0-.47-.528ZM8 4.5a.5.5 0 0 0-.5.5v8.5a.5.5 0 0 0 1 0V5a.5.5 0 0 0-.5-.5Z"/>
                      </svg>
                    </button>
                  </td>
                </tr>
              ))
            ) : (
              <tr>
                {/* Displayed when there are no departments */}
                <td colSpan='3'>No departments available.</td>
              </tr>
            )}
          </tbody>
        </table>

        {/* Modal for adding/editing departments */}
        <div className='modal fade' id='exampleModal' tabIndex='-1' aria-hidden='true'>
          <div className='modal-dialog modal-lg modal-dialog-centered'>
            <div className='modal-content'>
              <div className='modal-header'>
                <h5 className='modal-title'>{modalTitle}</h5>
                <button
                  type="button"
                  className='btn-close'
                  data-bs-dismiss='modal'
                  aria-label='Close'
                ></button>
              </div>
              <div className='modal-body'>
                <div className='input-group mb-3'>
                  <span className='input-group-text'>DepartmentName</span>
                  <input
                    type='text'
                    className='form-control'
                    value={DepartmentName}
                    onChange={this.changeDepartmentName}
                  />
                </div>

                {/* Create button (for adding new department) */}
                {DepartmentId === 0 ? (
                  <button
                    type='button'
                    className='btn btn-primary float-start'
                    onClick={() => this.createClick()}
                  >
                    Create
                  </button>
                ) : null}

                {/* Update button (for editing existing department) */}
                {DepartmentId !== 0 ? (
                  <button
                    type='button'
                    className='btn btn-primary float-start'
                    onClick={() => this.updateClick()}
                  >
                    Update
                  </button>
                ) : null}
              </div>
            </div>
          </div>
        </div>
      </div>
    );
  }
}
