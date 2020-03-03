import React from 'react';
import Autosuggest from 'react-autosuggest';
import PropTypes from 'prop-types';

function autocompleteRenderInput({ addTag, suggestions, ...props }) {
  const handleOnChange = (e, { newValue, method }) => {
    if (method === 'enter') {
      e.preventDefault();
    } else {
      props.onChange(e);
    }
  };

  const inputValue = (props.value && props.value.trim().toLowerCase()) || '';
  const inputLength = inputValue.length;

  let suggestionFilters = [];
  if (suggestions && suggestions.length > 0) {
    suggestionFilters = suggestions.filter(
      suggestion =>
        suggestion.name.toLowerCase().slice(0, inputLength) === inputValue,
    );
  }

  return (
    <Autosuggest
      ref={props.ref}
      suggestions={suggestionFilters}
      shouldRenderSuggestions={value => value && value.trim().length > 0}
      getSuggestionValue={suggestion => suggestion.name}
      renderSuggestion={suggestion => <span>{suggestion.name}</span>}
      inputProps={{ ...props, onChange: handleOnChange }}
      onSuggestionSelected={(e, { suggestion }) => {
        addTag(suggestion.name);
      }}
      onSuggestionsClearRequested={() => {}}
      onSuggestionsFetchRequested={() => {}}
    />
  );
}

autocompleteRenderInput.propTypes = {
  addTag: PropTypes.any,
  onChange: PropTypes.func,
  value: PropTypes.any,
  ref: PropTypes.any,
  suggestions: PropTypes.any,
};

export default autocompleteRenderInput;
