import uuidv4 from "../utils/uuidv4";

const login = () => {
  cy.visit(Cypress.env("BASE_URL"));
  cy.get('[data-testid="sign-in-up-button"]', { withinSubject: null }).click();
  cy.origin(Cypress.env("AUTH_SERVER_BASE_URL"), () => {
    cy.get("#Input_Email", { timeout: 2000 }).type("westcrime7777@gmail.com");
    cy.get("#Input_Password", { timeout: 2000 }).type("123456");
    cy.contains("button", "Login").click();
  });
  cy.contains("Offices", { timeout: 2000 }).click();
};

const createOffice = (address: string) => {
  cy.get('[data-testid="create-button"]').click();
  cy.get("#address-input-for-create-office-form", { timeout: 2000 }).type(
    address
  );
  cy.get("#registry-phone-number-input-for-create-office-form", {
    timeout: 2000,
  }).type("+375123456789");
  cy.get("#is-active-checkbox-input-for-create-office-form", {
    timeout: 2000,
  }).check();
  cy.contains("button", "Submit").click();
  cy.contains("span", "Creating: Successfully created").should("exist");
};

const processPage = (address: string, action) => {
  cy.get('[data-testid="office-card"]').then(($cards) => {
    let isOfficeFound = false;

    $cards.each((index, card) => {
      const cardText = Cypress.$(card).text().trim();
      if (cardText.includes(address) && !isOfficeFound) {
        action(card);
        isOfficeFound = true;
      }
    });

    if (!isOfficeFound) {
      cy.get('[data-testid="has-next-page-button"]').click();
      cy.wait(200);
      processPage(address, action);
    }
  });
};

describe("OfficesPage", () => {
  it("can't get offices when not authorized", () => {
    cy.visit("http://localhost:3000/offices");
    cy.get('[data-testid="office-card"]').should("not.exist");
  });

  it("logs in and gets offices", () => {
    login();
    cy.get('[data-testid="office-card"]').should("exist");
  });

  it("logs in and creates office", () => {
    login();
    const address = uuidv4();
    createOffice(address);
  });

  it("logs in and creates, deactivates and edits office", () => {
    login();
    const address = uuidv4();
    createOffice(address);
    processPage(address, (card) => {
      cy.wrap(card).find('[data-testid="deactivate-button"]').click();
      cy.get('[data-testid="confirm-button"]').click();
      cy.contains("span", "Deactivating: Successfully deactivated").should(
        "exist"
      );
      cy.wrap(card).find('[data-testid="edit-button"]').click();
      cy.get("#is-active-checkbox-input-for-edit-office-form", {
        timeout: 2000,
      }).check();
      cy.contains("button", "Submit").click();
      cy.contains("span", "Editing: Successfully edited").should("exist");
    });
  });

  it("logs in can not deactivate already deactivated office", () => {
    login();
    const address = uuidv4();
    createOffice(address);
    processPage(address, (card) => {
      cy.wrap(card).find('[data-testid="deactivate-button"]').click();
      cy.get('[data-testid="confirm-button"]').click();
      cy.contains("span", "Deactivating: Successfully deactivated").should(
        "exist"
      );
      cy.wrap(card)
        .find('[data-testid="deactivate-button"]')
        .should("be.disabled");
    });
  });

  it("logs in can not submit editing without changes", () => {
    login();
    const address = uuidv4();
    createOffice(address);
    processPage(address, (card) => {
      cy.wrap(card).find('[data-testid="edit-button"]').click();
      cy.contains("button", "Submit").should("be.disabled");
    });
  });
});
