import { render, screen } from "@testing-library/react";
import { describe, it, expect, vi } from "vitest";
import { CreateOfficeForm } from "@features/create-office-form";
import { Provider } from "react-redux";
import { MemoryRouter } from "react-router-dom";
import store from "@app/store";

const renderCreateOfficeForm = () =>
  render(
    <Provider store={store}>
      <MemoryRouter>
        <CreateOfficeForm close={vi.fn()} />
      </MemoryRouter>
    </Provider>
  );

describe("CreateOfficeForm", () => {
  it("renders", async () => {
    renderCreateOfficeForm();

    expect(screen.getByTestId("create-office-form")).toBeInTheDocument();
  });

  it("fields are enabled", async () => {
    renderCreateOfficeForm();

    const addressInput = screen.getByTestId("create-office-form-address-input");
    expect(addressInput).toBeInTheDocument();
    expect(addressInput).toBeEnabled();
    const phoneNumber = screen.getByTestId(
      "create-office-form-phone-number-input"
    );
    expect(phoneNumber).toBeInTheDocument();
    expect(phoneNumber).toBeEnabled();
    const isActive = screen.getByTestId(
      "create-office-form-is-active-checkbox"
    );
    expect(isActive).toBeInTheDocument();
    expect(isActive).toBeEnabled();
  });

  it("submit and cancel buttons are enabled", async () => {
    renderCreateOfficeForm();

    const addressInput = screen.getByTestId("submit-button");
    expect(addressInput).toBeInTheDocument();
    expect(addressInput).toBeEnabled();
    const phoneNumber = screen.getByTestId("cancel-button");
    expect(phoneNumber).toBeInTheDocument();
    expect(phoneNumber).toBeEnabled();
  });
});
