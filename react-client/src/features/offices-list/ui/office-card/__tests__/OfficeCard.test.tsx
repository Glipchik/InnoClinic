import { render, waitFor } from "@testing-library/react";
import OfficeModel from "@shared/models/offices/officeModel";
import { MemoryRouter } from "react-router-dom";
import OfficeCard from "..";
import { mockOfficeModel, mockOnDelete } from "./__mocks__";
import userEvent from "@testing-library/user-event";

const renderOfficeCard = (office: OfficeModel, onDelete: () => void) =>
  render(
    <MemoryRouter>
      <OfficeCard item={office} onDelete={onDelete} />
    </MemoryRouter>
  );

describe("OfficeCard", () => {
  it("renders office card with correct data", async () => {
    const { getByTestId, getByText } = renderOfficeCard(
      mockOfficeModel,
      mockOnDelete
    );

    await waitFor(() => {
      expect(getByTestId("office-card")).toBeInTheDocument();
      expect(getByText("Address: 123 Main St")).toBeInTheDocument();
      expect(getByText("Phone: +1234567890")).toBeInTheDocument();
      expect(getByText("Is active: Yes")).toBeInTheDocument();
      expect(getByTestId("edit-button")).toBeInTheDocument();
      expect(getByTestId("deactivate-button")).toBeInTheDocument();
    });
  });

  it("calls onDelete when deactivate button is clicked", async () => {
    const { getByTestId } = renderOfficeCard(mockOfficeModel, mockOnDelete);

    await waitFor(async () => {
      const deactivateButton = getByTestId("deactivate-button");
      await userEvent.click(deactivateButton);
      expect(mockOnDelete).toHaveBeenCalledTimes(1);
    });
  });

  it("disables deactivate button when office is not active", async () => {
    const inactiveOffice: OfficeModel = {
      ...mockOfficeModel,
      isActive: false,
    };

    const { getByTestId } = renderOfficeCard(inactiveOffice, mockOnDelete);

    await waitFor(async () => {
      const deactivateButton = getByTestId("deactivate-button");
      expect(deactivateButton).toBeDisabled();
    });
  });

  it("renders edit button with correct link", async () => {
    const { getByTestId } = renderOfficeCard(mockOfficeModel, mockOnDelete);

    await waitFor(async () => {
      const editButton = getByTestId("edit-button");
      expect(editButton.closest("a")).toHaveAttribute("href", "/offices/edit/1");
    });
  });
});
