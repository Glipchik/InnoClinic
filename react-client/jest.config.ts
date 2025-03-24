module.exports = {
  testEnvironment: "jsdom",
  testPathIgnorePatterns: ["/__mocks__/"],
  setupFilesAfterEnv: ["<rootDir>/jest.setup.ts"],
  moduleNameMapper: {
    "\\.(css|less|scss|sass)$": "identity-obj-proxy",

    "@features/(.*)": "<rootDir>/src/features/$1",
    "@pages/(.*)": "<rootDir>/src/pages/$1",
    "@shared/(.*)": "<rootDir>/src/shared/$1",
    "@entities/(.*)": "<rootDir>/src/entities/$1",
    "@app/(.*)": "<rootDir>/src/app/$1",
    "@models/(.*)": "<rootDir>/src/shared/models/$1",
    "@widgets/(.*)": "<rootDir>/src/widgets/$1",
  },
  transform: {
    "^.+\\.[t|j]sx?$": "babel-jest",
  },
};
