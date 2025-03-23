import { render, waitFor } from "@testing-library/react";
import { CreateOfficeForm } from "@features/create-office-form";
import { Provider } from "react-redux";
import { Store } from "@reduxjs/toolkit";
import { MemoryRouter } from "react-router-dom";
import { createMockStore, mockOnClose } from "./__mocks__";
import userEvent from "@testing-library/user-event";
import store from "@app/store";
import officesApi from "../api/offices";

const renderCreateOfficeForm = (store: Store) =>
  render(
    <Provider store={store}>
      <MemoryRouter>
        <CreateOfficeForm close={mockOnClose} />
      </MemoryRouter>
    </Provider>
  );

describe("CreateOfficeForm", () => {
  it("renders", async () => {
    const { getByTestId } = renderCreateOfficeForm(
      createMockStore({
        loading: false,
      })
    );

    await waitFor(() => {
      expect(getByTestId("create-office-form")).toBeInTheDocument();
      expect(getByTestId("create-office-form")).toBeInTheDocument();
      expect(
        getByTestId("create-office-form-address-input")
      ).toBeInTheDocument();
      expect(
        getByTestId("create-office-form-phone-number-input")
      ).toBeInTheDocument();
      expect(
        getByTestId("create-office-form-is-active-checkbox")
      ).toBeInTheDocument();
      expect(getByTestId("submit-button")).toBeInTheDocument();
      expect(getByTestId("cancel-button")).toBeInTheDocument();
      expect(
        getByTestId("create-office-form-is-active-checkbox")
      ).toBeInTheDocument();
    });
  });

  it("calls onClose when close button is clicked", async () => {
    const { getByTestId } = renderCreateOfficeForm(
      createMockStore({
        loading: false,
      })
    );

    await waitFor(async () => {
      const closeButton = getByTestId("cancel-button");
      await userEvent.click(closeButton);
      expect(mockOnClose).toHaveBeenCalledTimes(1);
    });
  });

  it("calls submit handler when form is submitted with valid data", async () => {
    const { getByTestId } = renderCreateOfficeForm(store);
    const postSpy = jest.spyOn(officesApi, "post");
    const addressInput = getByTestId(
      "create-office-form-address-input"
    ) as HTMLInputElement;
    const phoneInput = getByTestId(
      "create-office-form-phone-number-input"
    ) as HTMLInputElement;
    const submitButton = getByTestId("submit-button");

    await userEvent.type(addressInput, "123 Main St");
    await userEvent.type(phoneInput, "+375293061111");

    await userEvent.click(submitButton);

    await waitFor(() => {
      expect(addressInput.value).toBe("123 Main St");
      expect(phoneInput.value).toBe("+375293061111");
      expect(postSpy).toHaveBeenCalledWith({
        address: "123 Main St",
        registryPhoneNumber: "+375293061111",
        isActive: true,
      });
    });
  });

  it("displays validation errors when form is submitted with invalid data", async () => {
    const store = createMockStore({
      loading: false,
      success: false,
    });
    const { getByTestId, getByText } = renderCreateOfficeForm(store);

    const submitButton = getByTestId("submit-button");
    await userEvent.click(submitButton);

    await waitFor(() => {
      expect(getByText("Address is required")).toBeInTheDocument();
      expect(getByText("Phone number is required")).toBeInTheDocument();
    });
  });
});
